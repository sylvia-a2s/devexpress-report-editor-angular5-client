import { Component, ViewChild, AfterViewInit, ElementRef, Renderer2, Input } from '@angular/core';
import * as ko from 'knockout';
import { Html } from 'devexpress-reporting/dx-report-designer';

@Component({
  selector: 'report-designer',
  templateUrl: './report-designer.component.html'
})

export class ReportDesignerComponent implements AfterViewInit {
  koReportUrl = ko.observable(null);
  _reportUrl;
  designerOptions;
  constructor(private renderer: Renderer2) { }

  @ViewChild('scripts')
  scripts: ElementRef;

  @ViewChild('control')
  control: ElementRef;

  ngAfterViewInit() {
    const reportUrl = this.koReportUrl,
    host = 'http://localhost:49595/',
    container = this.renderer.createElement('div');
    container.innerHTML = Html;
    this.renderer.appendChild(this.scripts.nativeElement, container);
    this.designerOptions = {
      reportUrl,
      requestOptions: { // Options for processing requests from the Report Designer.
        host, // URI of your backend project.
        getDesignerModelAction: '/ReportDesigner/GetReportDesignerModel' // Action that returns the Report Designer model.
     }
    };
    ko.applyBindings(this.designerOptions, this.control.nativeElement);
  }

  Reset() {
    this.koReportUrl('2');
    // window.location.reload();
  }

  @Input()
  set reportUrl(reportUrl: string) {
    this._reportUrl = reportUrl;
    this.koReportUrl(reportUrl);
  }
  get reportUrl() {
    return this._reportUrl;
  }
}
