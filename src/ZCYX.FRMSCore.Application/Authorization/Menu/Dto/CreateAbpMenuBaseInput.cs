using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore
{
    [AutoMapTo(typeof(AbpMenuBase))]
    public class CreateAbpMenuBaseInput
    {

        public long? ParentId { get; set; }

        public string DisplayName { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }


        public int Order { get; set; }

        public string MoudleName { get; set; }

        //
        // 摘要:
        //     Can be used to enable/disable a menu item.
        public bool IsEnabled { get; set; }
        //
        // 摘要:
        //     Can be used to store a custom object related to this menu item. Optional.
        public string CustomData { get; set; }
        //
        // 摘要:
        //     Target of the menu item. Can be "_blank", "_self", "_parent", "_top" or a frame
        //     name.
        public string Target { get; set; }
        //
        // 摘要:
        //     This can be set to true if only authenticated users should see this menu item.
        public bool RequiresAuthentication { get; set; }


        //
        // 摘要:
        //     Can be used to show/hide a menu item.
        public bool IsVisible { get; set; }


        //
        // 摘要:
        //     The URL to navigate when this menu item is selected. Optional.
        public string Url { get; set; }


        public string RequiredPermissionName { get; set; }
    }
}
