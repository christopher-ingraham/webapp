const commander = require('commander');
const colors = require('colors');
const fs = require('fs');
const path = require('path');

function classify(dashedName) {
    const names = dashedName.split('-');
    return names.map((name) => name.charAt(0).toUpperCase() + name.slice(1)).join('');
}

class DirectoryEntry {
    constructor(name, parent) {
        this.name = name;
        this.fullName = path.join(parent, name);
    }
}

class DocumentEntry {
    constructor( directoryEntry, baseEntity, optionalSubType, optionalBaseClass) {
        this.baseEntity = baseEntity;
        this.fullEntity = optionalSubType ? `${baseEntity}-${optionalSubType}` : baseEntity;
        this.subType = optionalSubType;
        this.baseClass = optionalBaseClass;
        this.importExportName = this.fullEntity + '.class';
        this.name = this.importExportName + '.ts';
        this.fullName = path.join(directoryEntry.fullName, this.name)
    }
    create() {
        const className = classify(this.fullEntity);
        const lines = [`// ${this.name}`];
        if (this.baseClass) {
            const baseClassName = classify(this.baseClass.fullEntity);
            lines.push(`import { ${baseClassName} } from './${this.baseClass.importExportName}';`, '');
            lines.push(`export class ${className} extends ${baseClassName} {`, '');
        } else {
            lines.push(`export class ${className} {`, '', '    // TODO', '');
        }
        lines.push('}', '');
        const fileContent = lines.join('\r\n');
        fs.writeFileSync(this.fullName, fileContent);
    }
}

class IndexEntry {
    constructor(directoryEntry, documentList) {
        this.fullFileName = path.join(directoryEntry.fullName, 'index.ts');
        this.items = documentList || [];
    }
    create() {
        const lines = this.items.map((docInfo) => `export * from './${docInfo.importExportName}';`);
        const fileContent = lines.join('\r\n');
        fs.writeFileSync(this.fullFileName, fileContent);
    }
}

const program = new commander.Command();

program.requiredOption('-t, --target <directory>', 'base path to create DTOs and models');
program.requiredOption('-e, --entity <entity-name>', 'dashed name of the entity/type');
program.option('--dry-run', 'do not actually perform actions', false);
program.option('-v, --verbose', 'enable verbose logging');
program.parse(process.argv);

const dirDto = new DirectoryEntry('dto', program.target);
const dirModel = new DirectoryEntry('model', program.target);
const dirList = [dirDto, dirModel, ];

const entityBaseClass = new DocumentEntry(dirModel, program.entity, 'base');
const entityClass = new DocumentEntry(dirModel, program.entity, undefined, entityBaseClass);
const entityForInsertClass = new DocumentEntry(dirModel, program.entity, 'for-insert', entityBaseClass);
const entityForUpdateClass = new DocumentEntry(dirModel, program.entity, 'for-update', entityBaseClass);
const entityDetailClass = new DocumentEntry(dirModel, program.entity, 'detail', entityClass);
const entityListItemClass = new DocumentEntry(dirModel, program.entity, 'list-item', entityClass);
const entityListFilterClass = new DocumentEntry(dirModel, program.entity, 'list-filter');

const dtoList = [
];

const modelList = [
    entityBaseClass,
    entityClass,
    entityDetailClass,
    entityForInsertClass,
    entityForUpdateClass,
    entityListFilterClass,
    entityListItemClass,
];

const indexList = [
    new IndexEntry(dirDto, dtoList),
    new IndexEntry(dirModel, modelList),
];

dirList.forEach((dirInfo) => {
    if (program.verbose) {
        console.log(`create directory "${colors.green(dirInfo.fullName)}"`);
    }
    if (!program.dryRun) {
        fs.mkdirSync(dirInfo.fullName);
    }
});

indexList.forEach((indexInfo) => {
    indexInfo.items.forEach((docInfo) => {
        if (program.verbose) {
            console.log(`create document "${colors.green(docInfo.fullName)}"`);
        }
        if (!program.dryRun) {
            docInfo.create();
        }
    });
    if (program.verbose) {
        console.log(`create index "${colors.green(indexInfo.fullFileName)}"`);
    }
    if (!program.dryRun) {
        indexInfo.create();
    }
});

if (program.dryRun) {
    console.log(`Dry run set: ${colors.yellow('no action performed')}.`)
}

if (program.verbose) {
    console.log('done');
}
