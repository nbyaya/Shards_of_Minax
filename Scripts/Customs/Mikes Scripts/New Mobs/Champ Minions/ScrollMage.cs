using System;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of a scroll mage")]
    public class ScrollMage : BaseCreature
    {
        private TimeSpan m_SpellDelay = TimeSpan.FromSeconds(15.0); // time between spells
        public DateTime m_NextSpellTime;

        [Constructable]
        public ScrollMage() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Scroll Mage";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Scroll Mage";
            }

            Item robe = new Robe();
            robe.Hue = Utility.RandomBlueHue();
            AddItem(robe);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(150, 200);
            SetDex(80, 100);
            SetInt(250, 300);

            SetHits(200, 250);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 80.1, 90.0);
            SetSkill(SkillName.MagicResist, 85.0, 95.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 50.1, 60.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            m_NextSpellTime = DateTime.Now + m_SpellDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpellTime)
            {
                Mobile target = this.Combatant as Mobile;

                if (target != null && target.Map == this.Map && target.InRange(this, 10))
                {
                    CastRandomScrollSpell(target);
                    m_NextSpellTime = DateTime.Now + m_SpellDelay;
                }
            }

            base.OnThink();
        }

        private void CastRandomScrollSpell(Mobile target)
        {

        }

        public override void GenerateLoot()
        {
            PackGold(150, 200);
            AddLoot(LootPack.Rich);

            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));
            PackItem(new BlackPearl(Utility.RandomMinMax(5, 10)));
        }

        public ScrollMage(Serial serial) : base(serial)
        {
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
