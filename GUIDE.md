# DateTrigger Usage Guide

1. Create a single empty GameObject with the `Date Updater` component
    - (Optional) set method of retrieving date/time
    - Only one should be present, except if you want different methods
        - WARNING: LARGE AMOUNT OF THESE COMPONETS MIGHT LAG THE USERS!!!
2. Add component `Date Trigger` to the target object
    - if the object doesn't have an Animator Component, then add one
3. set updater
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
