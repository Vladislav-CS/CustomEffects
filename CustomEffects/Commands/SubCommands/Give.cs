﻿namespace CustomEffects.Commands.SubCommands
{
    using System.Linq;
    using CustomEffects.Effects;
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    
    public sealed class Give : ICommand
    {
        public string Command => "give";
        public string[] Aliases => new[]{ "gv" };
        public string Description => "Gives a custom effect to a player";

        public const string Usage = "Usage: ce give (player ID) (CustomEffect ID)";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission($"ce.{Command}"))
            {
                response = $"You do not have permission to execute this command. Permission: ce.{Command}";
                return false;
            }
            
            if (arguments.Count != 2 || !int.TryParse(arguments.At(0), out int playerId) || !uint.TryParse(arguments.At(1), out uint effectId))
            {
                response = Usage;
                return false;
            }

            Player effectedPlayer = Player.Get(playerId);

            if (effectedPlayer == null)
            {
                response = "Player was not found";
                return false;
            }

            CustomEffect customEffect;
            
            if (CustomEffect.Registered.Any(ce => ce.Id == effectId))
            {
                customEffect = CustomEffect.Registered.FirstOrDefault(ce => ce.Id == effectId);
            }
            else
            {
                response = "This custom effect was not found";
                return false;
            }

            customEffect.EnableEffects(effectedPlayer);
            
            response = $"Player {effectedPlayer.Nickname} got this custom effect";
            return true;
        }
    }
}