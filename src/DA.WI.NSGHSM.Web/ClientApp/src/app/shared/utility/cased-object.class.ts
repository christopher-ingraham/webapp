export class CasedObject {

    public static camelCase(inputObject: any, recursively = false) {
        const outputObject = {};
        Object.keys(inputObject).forEach((oldKey: string) => {
            const newKey = oldKey.charAt(0).toLowerCase() + oldKey.substring(1);
            let keyValue = inputObject[oldKey];
            if (recursively) {
                if ((typeof keyValue !== 'string') &&
                    (typeof keyValue !== 'number') &&
                    (typeof keyValue !== 'boolean') &&
                    (keyValue !== null) &&
                    (typeof keyValue !== 'undefined')) {
                    keyValue = CasedObject.camelCase(inputObject[oldKey], true);
                }
            }
            Object.assign(outputObject, { [newKey]: keyValue });
        });
        return outputObject;
    }

}
