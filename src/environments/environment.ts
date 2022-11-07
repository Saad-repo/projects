import { EnvironmentInterface } from "./environment-interface";

// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment: EnvironmentInterface = {
  production: false,
  //apiUrl: 'http://[2601:247:4300:44b:b8b2:4cf1:7d63:1832]/api1/api/',
  apiUrl: 'https://localhost:7083/api/', // 'http://localhost:5083',
  cdnUrl: 'assets/', // 'https://datasense.dev/cdn/app1/',
  headerNameIp: "cl-id",
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
 