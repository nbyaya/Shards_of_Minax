using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ShadeTheBlazingShadeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Shade the BlazingShade"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Sylvie Nightfall*, the enigmatic innkeeper of **The Ember Rest**.\n\n" +
                    "Her dark eyes flicker like coals in low light, and a faint scent of woodsmoke clings to her robes.\n\n" +
                    "“I’ve tended to this place since my mother passed, and she from hers—but now... something stirs beneath. Patrons have fled, murmuring of ghostly flames flickering from the cellar.”\n\n" +
                    "“I know what it is. *BlazingShade.* A cursed fire spirit, same as the one my great-grandmother once sheltered—and banished. I fear it’s come back. Or perhaps, it never left.”\n\n" +
                    "**Banish the BlazingShade** in the Gate of Hell dungeon, before it consumes the Ember Rest in flames unholy.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then my cellar shall burn in silence, and the Rest shall no longer be a refuge.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it haunts the deep? The fire creeps closer. Even now, the floorboards groan with heat.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it, then? The flames are still, and the shadows quiet.\n\n" +
                       "My patrons will return, and the Ember Rest will breathe again.\n\n" +
                       "Take this, **EarthRelic**, a keepsake of my family’s bond to fire and stone. Let it ground you, as we are grounded now, free from the blaze.";
            }
        }

        public ShadeTheBlazingShadeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BlazingShade), "BlazingShade", 1));
            AddReward(new BaseReward(typeof(EarthRelic), 1, "EarthRelic"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Shade the BlazingShade'!");
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

    public class SylvieNightfall : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ShadeTheBlazingShadeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTavernKeeper());
        }

        [Constructable]
        public SylvieNightfall()
            : base("the Ember Rest Innkeeper", "Sylvie Nightfall")
        {
        }

        public SylvieNightfall(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1145; // Pale, moonlit skin tone
            HairItemID = 0x2047; // Long hair
            HairHue = 1109; // Deep ash-black
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1154, Name = "Ashen Flame Gown" }); // Dark charcoal with ember trim
            AddItem(new HoodedShroudOfShadows() { Hue = 2101, Name = "Cinderveil Hood" }); // Deep crimson veil
            AddItem(new ThighBoots() { Hue = 1109, Name = "Nightfall Boots" }); // Dark black
            AddItem(new BodySash() { Hue = 1358, Name = "Emberwoven Sash" }); // Fiery orange-red
            AddItem(new RingmailArms() { Hue = 2405, Name = "Burnished Armlets" }); // Smoked bronze
            AddItem(new Scepter() { Hue = 2101, Name = "Smoldering Rod" }); // Scepter with faint glow effect

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Innkeeper's Satchel";
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
