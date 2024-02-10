using URCP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace URCP.Web.ModelExtensions
{
    public static class RolesTreeViewExtensions
    {
        public static RolesTreeViewModel FindNode(this List<RolesTreeViewModel> list, string nodeValue)
        {
            RolesTreeViewModel result = null;

            foreach (var item in list)
            {
                result = item.FindNode(nodeValue);
                if (result != null)
                    return result;
            }
            return result;
        }
        public static RolesTreeViewModel FindNode(this RolesTreeViewModel node, string nodeValue)
        {
            if (node.id == nodeValue) return node;
            else if (node.children.Any())
                return node.children.FindNode(nodeValue);

            else return null;
        }
        public static List<RolesTreeViewModel> ToModel(this IEnumerable<SelectListItem> selectList, string[] selectedRoles)
        {
            List<RolesTreeViewModel> result = new List<RolesTreeViewModel>();

            selectList = selectList.OrderBy(m => m.Value.Length);

            foreach (var kvp in selectList)
            {
                RolesTreeViewModel current = new RolesTreeViewModel();

                current.id = kvp.Value;
                current.name = kvp.Text;
                current.selected = selectedRoles != null ? selectedRoles.Contains(kvp.Value) : false;

                string parentStr = kvp.Value.Remove(kvp.Value.LastIndexOf("/"));
                var parent = result.FindNode(parentStr);
                if (parent != null)
                {
                    current.root = parent.id;
                    parent.children.Add(current);
                }
                else
                    result.Add(current);
            }
            return result;
        }
    }
}