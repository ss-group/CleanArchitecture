import { version } from 'src/version';


export const environment = {
  production: true,
  authUrl: 'https://spa.softsquaregroup.com/ssru.identity',
  apiUrl: '/api/',
  reportUrl: 'https://reportsvr.softsquare.ga/report',
  timeStamp: version.timeStamp,
};
