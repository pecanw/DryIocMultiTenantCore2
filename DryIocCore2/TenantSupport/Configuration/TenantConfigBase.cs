using System;
using System.Diagnostics;
using System.Globalization;

namespace DryIocCore2.TenantSupport.Configuration
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public class TenantConfigBase
    {
        private CultureInfo _cultureInfo;
        private CultureInfo _uiCultureInfo;

        private TimeZoneInfo _timeZoneInfo;

        public TenantConfigBase()
        {
            _cultureInfo = CultureInfo.DefaultThreadCurrentCulture ?? CultureInfo.InstalledUICulture;
            _uiCultureInfo = CultureInfo.DefaultThreadCurrentUICulture ?? CultureInfo.InstalledUICulture;
            _timeZoneInfo = TimeZoneInfo.Local;
        }

        public string Name { get; set; }

        public string ConnectionStringName { get; set; }

        public CultureInfo CultureInfo => _cultureInfo;

        public string Culture
        {
            get => _cultureInfo?.Name;
            set => _cultureInfo = CultureInfo.GetCultureInfo(value);
        }

        public CultureInfo UiCultureInfo => _uiCultureInfo;

        public string UiCulture
        {
            get => _uiCultureInfo?.Name;
            set => _uiCultureInfo = CultureInfo.GetCultureInfo(value);
        }

        public TimeZoneInfo TimeZoneInfo => _timeZoneInfo;

        public string TimeZone
        {
            get => _timeZoneInfo?.Id;
            set => _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(value);
        }
    }
}