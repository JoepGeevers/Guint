

# Guint
Sure! `int`'s make pretty primary keys! They're fast! Easy to share! Easy to communicate!

## Yes, but...

- Now I can enumerate your primary keys. One little authorization oversight and all your data are belong to me, thank you
- And, ha! all your competitors and future investors can exactly see how many records you actually have, thanks again!
- And, no proper domain-centric architecture or CQS for you, my friend, with your database dictating id's. Back to 2005 🦕 do not collect $200 👋

## So what should I do?
Change the primary key to `Guid`

## But I can't!*
Then, use **Guint**

Take your `int` and transform it to an `Guid` and the other way around. So have your `int` as a primary key in your database, but expose it as a `Guid`

In other words: *Have your cake and eat it too!*

## Show me how
```
public async Task<IHttpActionResult> Get(Guid id)
    => id
        .ToInt()
        .Match(
            i => this.carService.Get(i),
            notfound => NotFound());
}
```

## Setup

1. Generate your personal secret. Go to https://dotnetfiddle.net/2x6cA6 or run `var secret = Guint.GenerateSecret();`

2. Initialize Guint at the start of your application with `Guint.Use(secret)`

3. To transform, use extension methods `.ToGuid`, `.ToInt`, `.ToIntOrDefault` or `.ToGuidOrExplode`


---
\* For reasons!
