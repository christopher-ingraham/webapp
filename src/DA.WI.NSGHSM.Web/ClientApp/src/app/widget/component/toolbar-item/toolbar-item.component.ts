import {
    Component,
    ElementRef,
    Input,
    TemplateRef,
    ViewChild,
    forwardRef,
} from '@angular/core';
import { ToolBarToolComponent } from '@progress/kendo-angular-toolbar';

// for reference: https://www.telerik.com/kendo-angular-ui/components/toolbar/control-types/#toc-custom-control-types

function outerWidth(element: any): number {
    let width = element.offsetWidth;
    const style = getComputedStyle(element);

    width += (parseFloat(style.marginLeft) || 0 + parseFloat(style.marginRight) || 0);
    return width;
}

@Component({
    providers: [{
        provide: ToolBarToolComponent,
        useExisting: forwardRef(() => ToolbarItemComponent)
    }],
    selector: 'app-toolbar-item',
    template: `
        <ng-template #toolbarTemplate>
          <ng-content></ng-content>
        </ng-template>
    `
})
export class ToolbarItemComponent extends ToolBarToolComponent {
    @Input() public text: string;

    @ViewChild('toolbarTemplate', { static: true }) public toolbarTemplate: TemplateRef<any>;
    @ViewChild('toolbarElement', { static: true }) public toolbarElement: ElementRef;

    public get outerWidth(): number {
        if (this.toolbarElement) {
            return outerWidth(this.toolbarElement.nativeElement);
        }
    }
}
