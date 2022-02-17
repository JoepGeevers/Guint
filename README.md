

# Guint
Sure! `int`'s make pretty primary keys! They're fast! Easy to share! Easy to communicate!

## Yes, but...

- Now I can enumerate your primary keys. One little authorization oversight and all your data are belong to me, thank you
- And, ha! all your competitors and future investors can exactly see how many records you actually have, thanks again!
- And, no proper Ports and Adapters-, Hexagonal-, Union- or Clean Architecure for you, my friend, with your database dictating id's. Back to 2005 🦕 do not collect $200 👋

## So what should I do?
Change the primary key to `Guid`

## But I can't!*
Then, use **Guint**

Take your `int` and transform it to an `Guid` and the other way around. So have your `int` as a primary key in your database, but expose it as a `Guid`

In other words: *Have your cake and eat it too!*

## Show me how
```
public Task<IHttpActionResult> Get(Guid id)
{
	var i = id.DecryptToInt(key, vector);

	return i.HasValue
		? Ok(this.carService.Get(i))
		: NotFound();
}
```
To obtain a valid key and vector, run
 `(var key, var vector) = Guint.Guint.GenerateKeyAndInitializationVector();`
Or go to https://dotnetfiddle.net/z8FFmN and run it there

---
\* For reasons!
