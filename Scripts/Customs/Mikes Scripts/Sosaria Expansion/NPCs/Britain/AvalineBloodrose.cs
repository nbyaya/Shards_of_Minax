using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ScarletShadowQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Scarlet Shadow"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Avaline Bloodrose*, Court Alchemist of Castle British.\n\n" +
                    "Her eyes shimmer like garnet under torchlight, and her robes flow like liquid crimson, edged in silver runes that pulse faintly.\n\n" +
                    "“The Vault... Preservation Vault 44... I once whispered life into its veins. Hemocraft was my art—refined, controlled, sublime. But now? A **HemoglintStryga** festers in its heart, drawn to the residual blood magic like a moth to flame.”\n\n" +
                    "“It slithers through the blood storage chambers, corrupting my work, twisting it with glinting malice. I can no longer reach the relics... nor continue my research.”\n\n" +
                    "“You must slay it. But heed this: speak the incantations I will give you, else its blood magic will undo you from within.”\n\n" +
                    "**Slay the HemoglintStryga** and free the Vault from its crimson grasp.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "“Cowardice is a poison of the mind. Yet I shall wait, for someone brave enough to cleanse the Vault.”";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "“You still live, yet the Vault still bleeds. Recite the incantations—do not let the Stryga drink your courage.”";
            }
        }

        public override object Complete
        {
            get
            {
                return "“The air feels lighter... the Vault sighs with relief.\n\n" +
                       "You’ve slain it—the Stryga’s glinting thirst is no more.”\n\n" +
                       "**Take this:** *MoonspunTunic*. Woven in starlit hours, it shall guard you from darker tides yet to come.”";
            }
        }

        public ScarletShadowQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(HemoglintStryga), "HemoglintStryga", 1));
            AddReward(new BaseReward(typeof(MoonspunTunic), 1, "MoonspunTunic"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Scarlet Shadow'!");
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

    public class AvalineBloodrose : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ScarletShadowQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAlchemist(this));
        }

        [Constructable]
        public AvalineBloodrose()
            : base("the Court Alchemist", "Avaline Bloodrose")
        {
        }

        public AvalineBloodrose(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 100);

            Female = true;
            Body = 0x191; // Female Human
            Race = Race.Human;

            Hue = 1001; // Pale
            HairItemID = 0x2049; // Long hair
            HairHue = 1157; // Deep Crimson
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1157, Name = "Blood-Thread Robe" }); // Deep crimson robe
            AddItem(new WizardsHat() { Hue = 1153, Name = "Moonveil Hat" }); // Darker crimson
            AddItem(new BodySash() { Hue = 1154, Name = "Silverweft Sash" }); // Silver accents
            AddItem(new Sandals() { Hue = 1150, Name = "Vaultstep Sandals" }); // Black sandals

            AddItem(new ArtificerWand() { Hue = 1175, Name = "Hemocraft Rod" }); // Glowing red wand

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Alchemist’s Satchel";
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
