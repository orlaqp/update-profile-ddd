using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core.Exceptions;
using SimpleValidator.Exceptions;

namespace Core.CQRS
{
    public class CommandResult
    {
        public CommandResult()
        {
            Success = true;
            Errors = new Dictionary<string, string>();
        }

        public bool Success { get; private set; }
        public IDictionary<string, string> Errors { get; private set; }

        public void Ok() {
            Success = true;
            Errors.Clear();
        }

        internal void NotFound() {
            Error("NotFound", "Not Found");
        }

        internal void Error(string code, string message) {
            Success = false;
            Errors.Clear();
            Errors.Add(code, message);
        }

        internal void MultipleErrors(IDictionary<string, string> errors) {
            if (errors == null) {
                errors = new Dictionary<string, string>();
            }

            Success = false;
            Errors = errors;
        }

        internal void UnexpectedError() {
            Success = false;
            Errors.Clear();
            Errors.Add("Unexpected", "An unexpected error happenned");
        }

        internal void ValidationErrors(ValidationException ex)
        {
            Success = false;
            Errors.Clear();

            if (ex == null || ex.Validator == null) {
                return;
            }

            foreach (var e in ex.Validator.Errors)
            {
                Errors.Add(e.Name, e.Message);
            }
        }
    }
}