<strong>AOPify</strong> provides basic AOP features with fluent style.

List of supported methods,
  - Until
  - Delay
  - While
  - WhenTrue
  - Log
  - Before
  - After
  - HowLong
  - Catch
  - CatchAndThrow
  - ProcessAsync
  - Run
  - Return
  - RegisterLogger
List of supported attributes with process modes and processors 
(sample usage exist in source code look at: https://github.com/ziyasal/AOPify/tree/master/AOPify.Aspects.ConsoleTests)
  - PreProcess  : with options 
      - PreProcessMode.OnBefore,
      - PreProcessMode.WithInputParameters
  - PostProcess : with options 
      - PostProcessMode.OnAfter,
      - PostProcessMode.WithReturnType,
      - PostProcessMode.OnError, 
      - PostProcessMode.HowLong