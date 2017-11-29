Some good links to understand/explore Retry & Circuit Breaker patterns(& some other related patterns) - 

1) http://rahulrajatsingh.com/2016/10/understanding-retry-pattern-with-exponential-back-off-and-circuit-breaker-pattern/ 

2) http://blog.rogatnev.net/2016/07/patterns-retry-vs-circuit-breaker.html 

3) https://docs.microsoft.com/en-us/azure/architecture/patterns/category/resiliency

4) https://github.com/App-vNext/Polly

5) https://github.com/App-vNext/Polly-Samples


TODO - 
Following stuffs are still pending w.r.t Polly -
1) Exploring HandleInner, ORInner, HandleResult and OrResult and incorporating all these into this project(if needed)
2) Exploring BulkHead Isolation policy and incorporating this into this project(if needed)
3) Exploring Circuitbreaker.Isolate() and CircuitBreaker.Reset() capabilities and incorporating all these into this project(if needed)
4) Exploring AdvancedCircuitBreaker policy and incorporating all these into this project(if needed)
6) Exploring wrapping a policy to a different call site(e.g. Avatar, Reputation) and incorporating all these into this project(if needed)
7) Exploring Policy<TResult> and ExecuteAndCapture and incorporating all these into this project(if needed)
8) Exploring Policy Keys and Context data and incorporating all these into this project(if needed)
9) Exploring continueOnCapturedContext in ExecuteAsync to run on a captured synchronization context and incorporating this into this project(if needed)