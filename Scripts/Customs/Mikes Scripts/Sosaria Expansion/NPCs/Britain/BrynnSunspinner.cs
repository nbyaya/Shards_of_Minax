using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SolarShutdownQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Solar Shutdown"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself before *Brynn Sunspinner*, Lightwarden of Castle British.\n\n" +
                    "She stands amid the courtyard’s dimming lanterns, her prism staff catching faint rays of light, refracting them like spectral flames.\n\n" +
                    "“When the vault darkened, so did we. The SolenDroneBeta—a machine of ancient design—has hijacked the solar conduits in *Preservation Vault 44*.”\n\n" +
                    "“Our archives below are lost to shadow, and worse, the drone's draining of the sun’s light affects more than just our vision. The very magic of this place begins to wane.”\n\n" +
                    "“I cannot leave my post, but you… you could find it, in those fractured halls. Strike it down and restore the light. Let the vault breathe again.”\n\n" +
                    "**Slay the SolenDroneBeta**, and bring light back to the depths.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the darkness linger... but know this: shadows stretch farther than we see.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still the halls remain dark? Each flicker here echoes its power. I feel it leeching from the sun itself.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done more than slay a drone—you’ve reignited hope. The light returns, and with it, our strength.\n\n" +
                       "Take this: *ShellforgeVest*. A gift, hardened in sunfire, to shield you against shadows yet to come.";
            }
        }

        public SolarShutdownQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(SolenDroneBeta), "SolenDroneBeta", 1));
            AddReward(new BaseReward(typeof(ShellforgeVest), 1, "ShellforgeVest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Solar Shutdown'!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class BrynnSunspinner : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SolarShutdownQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHolyMage()); // Lightwarden with magical insight
        }

        [Constructable]
        public BrynnSunspinner()
            : base("the Lightwarden", "Brynn Sunspinner")
        {
        }

        public BrynnSunspinner(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 95, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1001; // Radiant skin tone
            HairItemID = 0x203B; // Long Hair
            HairHue = 1153; // Pale golden
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1154, Name = "Sunfire Robe" }); // Soft yellow-white hue
            AddItem(new BodySash() { Hue = 1172, Name = "Radiant Sash" }); // Light blue accent
            AddItem(new Sandals() { Hue = 1150, Name = "Sun-Blessed Sandals" }); // Pale white

            AddItem(new WizardsHat() { Hue = 1153, Name = "Prism-Crested Hat" });

            AddItem(new MagicWand() { Hue = 1360, Name = "Prism Staff" }); // Glimmering wand acting as her staff

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Lightwarden's Satchel";
            AddItem(backpack);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
