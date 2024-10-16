using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;

namespace Server.Commands
{
    public class CreateStatueCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("CreateStatue", AccessLevel.GameMaster, new CommandEventHandler(CreateStatue_OnCommand));
        }

        [Usage("CreateStatue <statue type>")]
        [Description("Creates a statue of a targeted player. Valid statue types are Marble, Jade, and Bronze.")]
        public static void CreateStatue_OnCommand(CommandEventArgs e)
        {
            if (e.Length == 1)
            {
                string typeString = e.GetString(0);
                StatueType type;

                if (Enum.IsDefined(typeof(StatueType), typeString))
                {
                    type = (StatueType)Enum.Parse(typeof(StatueType), typeString, true);
                    e.Mobile.Target = new CreateStatueTarget(type);
                    e.Mobile.SendMessage("Target the player you want to create a statue of.");
                }
                else
                {
                    e.Mobile.SendMessage("Invalid statue type. Use Marble, Jade, or Bronze.");
                }
            }
            else
            {
                e.Mobile.SendMessage("Usage: [CreateStatue <statue type>");
            }
        }

        private class CreateStatueTarget : Target
        {
            private StatueType m_Type;

            public CreateStatueTarget(StatueType type) : base(-1, false, TargetFlags.None)
            {
                m_Type = type;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is PlayerMobile)
                {
                    PlayerMobile target = (PlayerMobile)targeted;
                    CharacterStatue statue = new CharacterStatue(target, m_Type);
                    CharacterStatuePlinth plinth = new CharacterStatuePlinth(statue);

                    statue.Plinth = plinth;
                    plinth.MoveToWorld(from.Location, from.Map);
                    statue.InvalidatePose();

                    statue.Sculpt(from);

                    from.SendMessage(String.Format("Created a {0} statue of {1}.", m_Type.ToString(), target.Name));
                }
                else
                {
                    from.SendMessage("You must target a player.");
                }
            }
        }
    }
}