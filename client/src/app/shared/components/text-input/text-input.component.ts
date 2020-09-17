import { Component, ElementRef, Input, OnInit, Self, ViewChild } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
export class TextInputComponent implements OnInit, ControlValueAccessor {   // because we want to get access to FormControlValue, then we are implementing this interface: ControlValueAccessor
  @ViewChild('input', {static: true}) input: ElementRef;
  @Input() type = 'text';
  @Input() label: string;
                                        // NgControl is what our Form Controls derive from
  constructor(@Self() public controlDir: NgControl) {   // @Self() this decorator is for angular dependecy injection and angular is gonna look for where to locate what it's gonna inject into itself. And if we already have a service activated somewhere in our application it's going to walk up the tree of the Dependency hierarchy looking for something that matches what we're injecting here controlDir: NgControl. But if we used the @Self decorator here it's only going to use this inside itself and not look for any other shared dependency that's already in use. So this guarantees that we're working with the the very specific control that we're injecting in here
    this.controlDir.valueAccessor = this;   // And what this does this binds this to our class and now we've got access to our control directive inside our component and we'll have access to it inside our template as well.
  }

  ngOnInit(): void {
    const control = this.controlDir.control;  // and then we will get access to what validators have been set on this particular control
    const validators = control.validator ? [control.validator] : [];
    const asyncValidators = control.asyncValidator ? [control.asyncValidator] : [];

    control.setValidators(validators);    // So the control that we pass from let's say our logging form is going to pass across it's validators to this input and it's going to set them at the same time
    control.setAsyncValidators(asyncValidators);
    control.updateValueAndValidity();           // Now we've got our component set up to use the ControlValueAccessor to what we can do next is go to our components templates and configure that
  }

  // tslint:disable-next-line: typedef
  onChange(evenet) {}

  // tslint:disable-next-line: typedef
  onTouched() {}

  writeValue(obj: any): void {
    this.input.nativeElement.value = obj || '';   // we are getting the value of our input and write it into this method and this gives our ControlValueAccessor access to the values that's into the input field
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }


}
