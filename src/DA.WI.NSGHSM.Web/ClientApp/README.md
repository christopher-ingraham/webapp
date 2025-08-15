# DA.WI.NSGHSM.Web

In `ClientApp` directory there is the real frontend Angular application.

## 1. Development servers

There are two *backend servers* to run when in development mode.

### 1.1. .NET Core API server

Run

```bash
npm run start-api
```

which starts `DA.WI.NSGHSM.Api`.

### 1.2. EasyMock API server

```bash
npm run start-easymock
```

which starts [easymock](https://www.npmjs.com/package/easymock)
with configuration taken from directory `ClientApp/mock`;

### 1.3. Angular web server

Finally, you need to start the development web server provided with
Angular:

```
npm run start-ng-mock
```

which starts `ng serve` with appropriate proxy configuration to access
the servers above. PRoxy configuration to be found in
`ClientApp/proxy-mock.conf.json`

Navigate to `http://localhost:4200/`. The DAWI app will automatically
reload if you change any of the source files.

## 2. Troubleshooting

To do.
