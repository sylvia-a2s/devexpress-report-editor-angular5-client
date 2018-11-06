import { Component, ViewChild, AfterViewInit, ElementRef, Renderer2, Input } from '@angular/core';
import * as ko from 'knockout';
import { Html, ReportDesigner } from 'devexpress-reporting/dx-report-designer';
import { calcBindingFlags } from '@angular/core/src/view/util';

@Component({
  selector: 'report-designer',
  templateUrl: './report-designer.component.html'
})

export class ReportDesignerComponent implements AfterViewInit {
  koReportUrl = ko.observable(null);
  // urls = ko.observableArray([]);
  _reportUrl;
  designerOptions;
  testnum = 3;
  currentNum;
  public listUrls = 'test';
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
     },
     callbacks: {
       ReportOpened: function(s, e) {
         this.currentNum = e.Url;
       },
       CustomizeOpenDialog: function(s, e) {
        // const urls = ko.observableArray([]);
        s.ReportStorageGetUrls().done(function (result) {
            const test = result;
            console.log(test);
        });
        // console.log(urls.map(res => res));
       }
      //  ReportSaved: function(s, e) {
      //     s.CloseCurrentTab();
      //  }
     }
    };
    ko.applyBindings(this.designerOptions, this.control.nativeElement);
  }

  Reset() {
    this.koReportUrl(this.testnum);
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
