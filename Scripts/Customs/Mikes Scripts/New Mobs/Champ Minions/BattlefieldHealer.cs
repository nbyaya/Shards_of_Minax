using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a healer")]
    public class BattlefieldHealer : BaseCreature
    {
        private TimeSpan m_HealDelay = TimeSpan.FromSeconds(5.0); // time between healing actions
        public DateTime m_NextHealTime;

        [Constructable]
        public BattlefieldHealer() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
            Body = 0x191;
            Name = NameList.RandomName("female");
            Title = " the Healer";

            Item robe = new Robe(Utility.RandomNeutralHue());
            AddItem(robe);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            Item sandals = new Sandals();
            AddItem(sandals);

            SetStr(500, 700);
            SetDex(100, 150);
            SetInt(300, 400);

            SetHits(400, 600);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Healing, 90.1, 100.0);
            SetSkill(SkillName.Anatomy, 90.1, 100.0);
            SetSkill(SkillName.Magery, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 70.0, 90.0);
            SetSkill(SkillName.Tactics, 50.0, 70.0);

            Fame = 5000;
            Karma = 5000;

            VirtualArmor = 30;

            m_NextHealTime = DateTime.Now + m_HealDelay;
        }

        public override bool AlwaysMurderer { get { return false; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextHealTime)
            {
                HealAllies();
                m_NextHealTime = DateTime.Now + m_HealDelay;
            }

            base.OnThink();
        }

        private void HealAllies()
        {
            foreach (Mobile ally in GetMobilesInRange(5))
            {
                if (ally != null && ally != this && ally.Hits < ally.HitsMax && !ally.Blessed)
                {
                    ally.Hits += Utility.RandomMinMax(10, 30); // Heal between 10 and 30 hit points
                    this.Say("Be healed, my friend!");
                    if (Utility.RandomBool())
                    {
                        ally.PlaySound(0x214);
                    }
                    else
                    {
                        ally.PlaySound(0x215);
                    }
                }
                else if (ally != null && ally != this && ally.Alive == false && !ally.Blessed)
                {
                    ally.Resurrect(); // Revive fallen ally
                    this.Say("Rise again!");
                    ally.PlaySound(0x214);
                }
            }
        }

        public BattlefieldHealer(Serial serial) : base(serial)
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
