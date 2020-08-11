import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import { ApplicationInsights } from '@microsoft/applicationinsights-web'

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');
const appInsights = new ApplicationInsights({
    config: {
        instrumentationKey: 'ddd1ddf4-8f62-40da-823d-73533cf02226'
        /* ...Other Configuration Options... */
    }
});
appInsights.loadAppInsights();
appInsights.trackPageView();

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <App />
  </BrowserRouter>,
  rootElement);

registerServiceWorker();

