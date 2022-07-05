namespace MyCompany.Meeting.Common.Domain

open System

module ConstrainedType =
    // Create a constrained string using the constructor provided
    // Return Error if input is null, empty, or length > maxLen
    let createString fieldName ctor maxLen str =
        if String.IsNullOrEmpty(str) then
            let msg = sprintf "%s must not be null or empty" fieldName
            Error msg
        elif str.Length > maxLen then
            let msg = sprintf "%s must not be more than %i chars" fieldName maxLen
            Error msg
        else
            Ok(ctor str)

    // Create a optional constrained string using the constructor provided
    // Return None if input is null, empty.
    // Return error if length > maxLen
    // Return Some if the input is valid
    let createStringOption fieldName ctor maxLen str =
        if String.IsNullOrEmpty(str) then
            Ok None
        elif str.Length > maxLen then
            let msg = sprintf "%s must not be more than %i chars" fieldName maxLen
            Error msg
        else
            Ok(ctor str |> Some)

    // Create a constrained integer using the constructor provided
    // Return Error if input is less than minVal or more than maxVal
    let createInt fieldName ctor minVal maxVal i =
        if i < minVal then
            let msg = sprintf "%s: Must not be less than %i" fieldName minVal
            Error msg
        elif i > maxVal then
            let msg = sprintf "%s: Must not be greater than %i" fieldName maxVal
            Error msg
        else
            Ok(ctor i)

    // Create a constrained decimal using the constructor provided
    // Return Error if input is less than minVal or more than maxVal
    let createDecimal fieldName ctor minVal maxVal d =
        if d < minVal then
            let msg = sprintf "%s: Must not be less than %M" fieldName minVal
            Error msg
        elif d > maxVal then
            let msg = sprintf "%s: Must not be greater than %M" fieldName maxVal
            Error msg
        else
            Ok(ctor d)

    // Create a constrained string using the constructor provided
    // Return Error if input is null. empty, or does not match the regex pattern
    let createLike fieldName ctor pattern str =
        if String.IsNullOrEmpty(str) then
            let msg = sprintf "%s: Must not be null or empty" fieldName
            Error msg
        elif System.Text.RegularExpressions.Regex.IsMatch(str, pattern) then
            Ok(ctor str)
        else
            let msg = sprintf "%s: '%s' must match the pattern '%s'" fieldName str pattern
            Error msg
