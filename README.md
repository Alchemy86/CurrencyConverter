# CurrencyConverter

Essentially a 24hr coding challenge requirement.

## Tech Stack
#Angular
- All UI done in Angular
- Debounce used - 1 second when setting the value
- Changes are instant - Audit log has a 1 second delay

#.net Core
- Back end .net core
- Not much structure, just some bare bones
- In memory DB 
- API called externally to get the current currency conversion rates - works with any, code locked down to GBP
- Audit of chosen conversions is recored in tempDB

#Cache
- Last converted value is cached
