# RS2-IS-Pozoriste

## Uputstva za pokretanje

### Lokalni razvoj

Instalirati pakete za klijentski modul:
```bash
cd Server/ClientApp/
npm install
cd ../../
```
Pokrenuti dotnet server, koji automatski pokreće frontend development server:
```bash
cd Server/
dotnet run
```

### Produkciono okruženje

Pripremiti statičke fajlove klijentskog modula:
```bash
cd Server/ClientApp/
npm install
npm run build
cd ../../
```
Pokrenuti server:
```bash
cd Server/
dotnet run --launch-profile=Prod
```
