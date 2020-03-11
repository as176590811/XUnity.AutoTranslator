﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using XUnity.AutoTranslator.Plugin.Core.Configuration;
using XUnity.AutoTranslator.Plugin.Core.Endpoints;
using XUnity.AutoTranslator.Plugin.Core.Endpoints.ExtProtocol;
using XUnity.AutoTranslator.Plugin.Core.Extensions;
using XUnity.AutoTranslator.Plugin.Core.Web;
using System.Runtime.InteropServices;

namespace ezTransXP
{
   public class ezTransXPEndpoint : ExtProtocolEndpoint
   {
      public override string Id => "ezTransXP";

      public override string FriendlyName => "ezTransXP";

      public override int MaxConcurrency => 1;

      public override int MaxTranslationsPerRequest => 50;

      public override void Initialize( IInitializationContext context )
      {
         var defaultPath = GetDefaultInstallationPath();
         var path = context.GetOrCreateSetting( "ezTrans", "InstallationPath", defaultPath );

         if( string.IsNullOrEmpty( path ) && !string.IsNullOrEmpty( defaultPath ) )
         {
            context.SetSetting( "ezTrans", "InstallationPath", defaultPath );
            path = defaultPath;
         }

         var exePath = Path.Combine( context.TranslatorDirectory, @"FullNET\ezTransXP.ExtProtocol.exe" );

         var fileExists = File.Exists( exePath );
         if( !fileExists ) throw new EndpointInitializationException( $"Could not find any executable at '{exePath}'" );

         ExecutablePath = exePath;
         Arguments = Convert.ToBase64String( Encoding.UTF8.GetBytes( path ) );

         if( context.SourceLanguage != "ja" ) throw new EndpointInitializationException( "Current implementation only supports japanese-to-korean." );
         if( context.DestinationLanguage != "ko" ) throw new EndpointInitializationException( "Current implementation only supports japanese-to-korean." );

      }
      public static string GetDefaultInstallationPath()
      {
         if( IntPtr.Size == 8 ) // 64-bit
         {
            return @"C:\Program Files (x86)\ChangShinSoft\ezTrans XP";
         }
         else // 32-bit
         {
            return @"C:\Program Files\ChangShinSoft\ezTrans XP";
         }
      }
   }
}
