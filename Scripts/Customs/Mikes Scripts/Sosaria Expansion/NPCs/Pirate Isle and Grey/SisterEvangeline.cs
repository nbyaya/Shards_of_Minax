using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SpritesLamentQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Sprite's Lament"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Sister Evangeline*, Choirmaster of Grey’s seaside cathedral.\n\n" +
                    "She gently adjusts a silvered staff, her voice like the hush of tides.\n\n" +
                    "“Do you hear it? The discord. A spirit of sorrow, a WailingSprite, has taken roost in Exodus Dungeon. Its cries reach even here, shattering glass, unraveling our hymns. My singers can no longer perform the rites.”\n\n" +
                    "“This is not just noise—it’s a lament of a soul unbound, feeding on harmony. I can soothe lesser spirits with hymns of the Silver Tongue... but not this one.”\n\n" +
                    "“Will you silence it? Set it free from this torment, and let our choirs breathe once more.”\n\n" +
                    "**Slay the WailingSprite** in Exodus Dungeon and bring peace to our songs.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then we must endure the silence, and pray the Sprite does not unravel more than song.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Its cries still haunt us. Each night they grow louder, more twisted.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it... the lament has ceased. The cathedral is still, as it should be.\n\n" +
                       "Take this: the *MysticalDaoChest*. May it keep your soul as steady as your hand.\n\n" +
                       "And should you hear whispers in the silence, know they are only echoes of gratitude.";
            }
        }

        public SpritesLamentQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(WailingSprite), "WailingSprite", 1));
            AddReward(new BaseReward(typeof(MysticalDaoChest), 1, "MysticalDaoChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Sprite's Lament'!");
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

    public class SisterEvangeline : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SpritesLamentQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBard()); // Choirmaster, musical instruments and song scrolls
        }

        [Constructable]
        public SisterEvangeline()
            : base("the Choirmaster", "Sister Evangeline")
        {
        }

        public SisterEvangeline(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 45);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale, moonlit skin
            HairItemID = 0x203B; // Long hair
            HairHue = 1153; // Silvery white
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1153, Name = "Moonlit Robe of the Choir" });
            AddItem(new Sandals() { Hue = 1150, Name = "Silent Steps" });
            AddItem(new Cloak() { Hue = 1154, Name = "Veil of Harmonies" });
            AddItem(new FeatheredHat() { Hue = 1152, Name = "Crest of the Silver Tongue" });

            AddItem(new GnarledStaff() { Hue = 0x482, Name = "Staff of Resonance" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Choral Satchel";
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
