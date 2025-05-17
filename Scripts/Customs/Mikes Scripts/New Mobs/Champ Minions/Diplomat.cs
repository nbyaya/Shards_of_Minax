using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a diplomat")]
    public class Diplomat : BaseCreature
    {
        private TimeSpan m_TruceDelay = TimeSpan.FromMinutes(1.0); // time between truces
        public DateTime m_NextTruceTime;

        [Constructable]
        public Diplomat() : base(AIType.AI_Mage, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Diplomat";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Diplomat";
            }

            Item robe = new Robe();
            robe.Hue = Utility.RandomBrightHue();
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

            SetStr(200, 300);
            SetDex(100, 150);
            SetInt(300, 400);

            SetHits(200, 300);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 80.1, 100.0);
            SetSkill(SkillName.MagicResist, 85.5, 100.0);

            Fame = 2000;
            Karma = 2000;

            VirtualArmor = 40;

            m_NextTruceTime = DateTime.Now + m_TruceDelay;
        }

        public override bool AlwaysMurderer { get { return false; } }
        public override bool CanRummageCorpses { get { return false; } }
        public override bool ShowFameTitle { get { return true; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextTruceTime)
            {
                foreach (Mobile m in this.GetMobilesInRange(8))
                {
                    if (m is BaseCreature && ((BaseCreature)m).Combatant == this)
                    {
                        ((BaseCreature)m).Combatant = null;
                        m.SendMessage("The Diplomat negotiates a truce, preventing you from attacking for a while.");
                    }
                }
                
                m_NextTruceTime = DateTime.Now + m_TruceDelay;
            }

            base.OnThink();
        }

        public override void GenerateLoot()
        {
            PackGold(100, 150);
            AddLoot(LootPack.Average);

            if (Utility.Random(2) == 0)
            {
                this.Say(true, "May peace prevail...");
            }
            else
            {
                this.Say(true, "A truce, for now...");
            }

            PackItem(new Gold(Utility.RandomMinMax(10, 20)));
        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    this.Say(true, "Let's find a peaceful resolution.");
                    m_NextTruceTime = DateTime.Now + m_TruceDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public Diplomat(Serial serial) : base(serial)
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
