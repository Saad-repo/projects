


Security
-----------------------------------------
HTTP 401 - Unauthorized is returned if request lacks security code or if security code is not valid.

Security Code is a dynamic value that's calculated and stored in an HTTP Header named **dynamic-ctx-type-code**

```js
static pi: number = 3.1415;

// return userId|(PI+DAY+HR)+PI*HR*MiN
  static getDynamicCode(): string {
    let date = new Date();
    let hours = date.getHours();
    let minutes = date.getMinutes();
    let day = date.getDay();
    let mins = Math.floor(minutes / 5);
    
    let dynamicValue =  (this.pi + day + hours)
          + this.pi * hours * mins;

    return `${this.userId}|${dynamicValue}`;
  }
```


