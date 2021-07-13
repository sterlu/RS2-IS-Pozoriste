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

Pokrenuti Payment mikroservis:
```bash
cd PaymentService/
dotnet run
```
Dodatno, za testiranje procesa plaćanja potrebno je podesiti i pokrenuti Stripe CLI:
```bash
stripe listen --forward-to https://localhost:5001/api/placanje/webhook --print-json
```

### Produkciono okruženje

Aplikacija je dokerizovana pa je pokretanje jednostavno:
```bash
docker-compose up --build
```