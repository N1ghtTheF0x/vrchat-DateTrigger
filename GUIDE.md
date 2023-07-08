# DateTrigger Usage Guide

1. Add component `Date Trigger` to object
2. if the object doesn't have an Animator Component, then add one
3. (Optional) set hour offset
4. (Optional) set check type
5. (Optional) set parameter for Animation
6. for each enabled option
    - set `To`
    - set `From`
    - if `To` and `From` are the same, then it will only check for that exact value
7. On that Animator create a parameter type bool named after the `Parameter` property
8. Create animation which use that parameter
9. Profit