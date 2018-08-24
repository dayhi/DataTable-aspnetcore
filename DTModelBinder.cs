using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace dotnet_backstage.Model {
    /// <summary>
    /// DataTables.net 1.10.19
    /// 传入
    /// </summary>
    public class DTModelBinder : IModelBinder {
        public Task BindModelAsync (ModelBindingContext bindingContext) {
            var query = bindingContext.HttpContext.Request.Query;
            var draw = Convert.ToInt32 (query["draw"]);
            var start = Convert.ToInt32 (query["start"]);
            var length = Convert.ToInt32 (query["length"]);
            // Search
            var search = new DataTablesSearch {
                Value = query["search[value]"],
                Regex = Convert.ToBoolean (query["search[regex]"])
            };
            // Order
            var o = 0;
            var order = new List<DataTablesOrder> ();
            while (!string.IsNullOrEmpty (query["order[" + o + "][column]"].ToString ())) {
                order.Add (new DataTablesOrder {
                    Column = Convert.ToInt32 (query["order[" + o + "][column]"]),
                        Dir = (DataTablesOrderDir) Enum.Parse (typeof (DataTablesOrderDir), query["order[" + o + "][dir]"])
                });
                o++;
            }
            // Columns
            var c = 0;
            var columns = new List<DataTablesColumns> ();
            while (!string.IsNullOrEmpty (query["columns[" + c + "][data]"].ToString ())) {
                columns.Add (new DataTablesColumns {
                    Data = query["columns[" + c + "][data]"],
                        Name = query["columns[" + c + "][name]"],
                        Orderable = Convert.ToBoolean (query["columns[" + c + "][orderable]"]),
                        Searchable = Convert.ToBoolean (query["columns[" + c + "][searchable]"]),
                        Search = new DataTablesSearch {
                            Value = query["columns[" + c + "][search][value]"],
                                Regex = Convert.ToBoolean (query["columns[" + c + "][search][regex]"])
                        }
                });
                c++;
            }

            var result = new DataTableRequestModel {
                Draw = draw,
                Start = start,
                Length = length,
                Search = search,
                Order = order,
                Columns = columns
            };
            bindingContext.Result = ModelBindingResult.Success (result);
            return Task.CompletedTask;
        }

    }
}