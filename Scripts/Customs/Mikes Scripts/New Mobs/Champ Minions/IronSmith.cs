using System;
using Server.Items;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("corpse of an iron smith")]
    public class IronSmith : BaseCreature
    {
        private TimeSpan m_BuffDelay = TimeSpan.FromSeconds(15.0); // time between buffs
        public DateTime m_NextBuffTime;

        [Constructable]
        public IronSmith() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = 2406; // Set to a metallic hue
            Body = 0x190; // Male body type
            Name = "an Iron Smith";
			Team = 2;

            // Equip the Iron Smith
            Item apron = new FullApron();
            apron.Hue = 2419;
            AddItem(apron);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = 1150; // Set to greyish hue
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item boots = new Boots(Utility.RandomNeutralHue());
            
            AddItem(hair);
            AddItem(pants);
            AddItem(boots);

            Item hammer = new WarHammer();
            AddItem(hammer);
            hammer.Movable = false;

            SetStr(800, 1200);
            SetDex(100, 200);
            SetInt(100, 200);

            SetHits(1000, 1500);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 80);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Blacksmith, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 80.1, 100.0);
            SetSkill(SkillName.MagicResist, 85.0, 95.0);
            SetSkill(SkillName.Wrestling, 70.1, 90.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 70;

            m_NextBuffTime = DateTime.Now + m_BuffDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextBuffTime)
            {
                List<Mobile> allies = new List<Mobile>();
                foreach (Mobile m in this.GetMobilesInRange(8))
                {
                    if (m != this && m is BaseCreature && ((BaseCreature)m).ControlMaster == this.ControlMaster)
                    {
                        allies.Add(m);
                    }
                }

                foreach (Mobile ally in allies)
                {
                    if (ally != null && ally.Alive && ally.Map == this.Map && ally.InRange(this, 8))
                    {
                        this.Say(true, "Your armor is now fortified!");
                        ally.VirtualArmorMod += 20;
                    }
                }

                m_NextBuffTime = DateTime.Now + m_BuffDelay;
            }

            base.OnThink();
        }

        public override void GenerateLoot()
        {
            PackGold(300, 400);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 2);
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

        public IronSmith(Serial serial) : base(serial)
        {
        }
    }
}
