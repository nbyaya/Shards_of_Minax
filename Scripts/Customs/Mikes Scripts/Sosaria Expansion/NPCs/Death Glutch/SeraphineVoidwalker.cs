using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FissureStalkerQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Fissure Stalker"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Seraphine Voidwalker*, Rune Carver of Death Glutch, her fingers stained with arcane ink.\n\n" +
                    "The air crackles faintly as she adjusts a hovering rune-stone, eyes fixed yet distant.\n\n" +
                    "“You see this?” she gestures to a half-finished ward. “It won’t hold. It *never* holds anymore.”\n\n" +
                    "“There’s something in the Malidor Witches Academy... something that *phases*—through walls, through my wards, through *space itself*. A beast, the locals whisper, but I know better. It’s not just a beast—it’s a flaw. A tear in the weave of reality.”\n\n" +
                    "**The Displacement Beast.**”\n\n" +
                    "“Every time it flickers, my runes fail. Every time it stalks, my craft weakens. I cannot hold this town if it unravels everything I bind.”\n\n" +
                    "“I believe my rune-tools—my own creations—may be the key to its end. Take them, hunt it, and close the rift with its blood.”\n\n" +
                    "**Slay the Displacement Beast** and return peace to the woven wards of Death Glutch.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the beast’s paths stay unseen—for now. But I will not be able to protect this place much longer.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You have not yet slain it? The distortions worsen. I can barely trace a straight ward... it laughs in the weave.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The rift closes... the air tightens again. You’ve slain it, haven’t you? I feel the wards *settle*.\n\n" +
                       "Take these: *Tamer’s Bindings*. May they grant you mastery where I lost control. And remember—some rifts, once opened, never truly fade.";
            }
        }

        public FissureStalkerQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DisplacementBeast), "Displacement Beast", 1));
            AddReward(new BaseReward(typeof(TamersBindings), 1, "Tamer’s Bindings"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Fissure Stalker'!");
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

    public class SeraphineVoidwalker : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FissureStalkerQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFortuneTeller()); // Closest to a rune carver
        }

        [Constructable]
        public SeraphineVoidwalker()
            : base("the Rune Carver", "Seraphine Voidwalker")
        {
        }

        public SeraphineVoidwalker(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 85, 100);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1154; // Pale, void-touched skin tone
            HairItemID = 0x2047; // Long Hair
            HairHue = 1150; // Dark violet hair
        }

        public override void InitOutfit()
        {
            AddItem(new DeathRobe() { Hue = 1175, Name = "Void-Touched Shroud" }); // Deep void-blue robe
            AddItem(new Cloak() { Hue = 1170, Name = "Phantom Veil" }); // Slightly lighter blue
            AddItem(new WizardsHat() { Hue = 1175, Name = "Runic Diadem" });
            AddItem(new Sandals() { Hue = 1175, Name = "Phasewalkers" });

            AddItem(new ArtificerWand() { Hue = 1175, Name = "Runebinder's Staff" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Rune Carver’s Satchel";
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
