import { AbstractControl, FormControl, FormGroup, ValidationErrors } from '@angular/forms'
import { concat } from 'rxjs';
 
export function matchPasswordValidator(control: AbstractControl): ValidationErrors | null {
 
    const password = control.get("password")!.value;
    const confirm = control.get("confirm")!.value;
    // const password = control1.value;
    // const confirm = control2.value;
 
    if(!password || !confirm) {
        return null;
    }

    //console.log(password,confirm);
    
    if (password != confirm) { 
        //console.log("yes, it's a mimatch");
        let fc = control.get("confirm") as FormControl;
        fc.setErrors({noMatch: "Password mismatch"});
        //control2.setErrors({error: "testest"});
        return { noMatch: true } 
    }

    //console.log("Noo1");
    return null;
 
}