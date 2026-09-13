## Release v0.0.1

### New Rules

| Rule ID    | Category | Severity | Notes                       |
|------------|----------|----------|-----------------------------|
| CFEMAPI001 | Mapping  | Error    | Missing ResultErrorMapper   |
| CFEMAPI002 | Mapping  | Error    | Duplicate ResultErrorMapper |

## Release v0.0.4

### Removed Rules

| Rule ID    | Category | Severity | Notes                     |
|------------|----------|----------|---------------------------|
| CFEMAPI001 | Mapping  | Error    | Missing ResultErrorMapper |

## Release v0.0.5

### New Rules

| Rule ID     | Category | Severity | Notes                       |
|-------------|----------|----------|-----------------------------|
| CFEHTTPR002 | Mapping  | Error    | Duplicate ResultErrorMapper |

### Removed Rules

| Rule ID    | Category | Severity | Notes                       |
|------------|----------|----------|-----------------------------|
| CFEMAPI002 | Mapping  | Error    | Duplicate ResultErrorMapper |

## Release v0.5.0

### New Rules

| Rule ID     | Category | Severity | Notes                                  |
|-------------|----------|----------|----------------------------------------|
| CFEHTTPR003 | Mapping  | Error    | Empty Map getter in IResultErrorMapper |

## Release v0.9.0

### Removed Rules

| Rule ID     | Category | Severity | Notes                                  |
|-------------|----------|----------|----------------------------------------|
| CFEHTTPR003 | Mapping  | Error    | Empty Map getter in IResultErrorMapper |

## Release v1.0.1

### New Rules

| Rule ID     | Category | Severity | Notes                                                   |
|-------------|----------|----------|---------------------------------------------------------|
| CFEHTTPR004 | Mapping  | Error    | Missing parameterless constructor in IResultErrorMapper |

## Release v2.0.0

### Changed Rules

| Rule ID     | Category | Severity | Notes                                                                                 |
|-------------|----------|----------|---------------------------------------------------------------------------------------|
| CFEHTTPR002 | Mapping  | Error    | Semantic duplicate groups are excluded without suppressing unrelated generated output |
| CFEHTTPR004 | Mapping  | Error    | Validates accessible construction and required members for result error mappers        |

### New Rules

| Rule ID     | Category | Severity | Notes                                                        |
|-------------|----------|----------|--------------------------------------------------------------|
| CFEHTTPR005 | Mapping  | Error    | Multiple valid IResultProblemDetailsProvider implementations |
| CFEHTTPR006 | Mapping  | Error    | Unsupported mapper type shape                                |
| CFEHTTPR007 | Mapping  | Error    | Unsupported provider type shape                              |
