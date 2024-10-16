using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a storm caller")]
    public class BattleStormCaller : BaseCreature
    {
        private TimeSpan m_StormDelay = TimeSpan.FromSeconds(20.0); // time between storms
        public DateTime m_NextStormTime;

        [Constructable]
        public BattleStormCaller() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = 1150; // Stormy gray hue
            Body = 0x190; // Human male body
            Name = "Storm Caller";

            Item robe = new Robe(1150); // Gray robe
            AddItem(robe);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = 1150; // Gray hair
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            Item boots = new Boots(1109); // Dark boots
            AddItem(boots);

            SetStr(600, 800);
            SetDex(150, 200);
            SetInt(400, 500);

            SetHits(500, 700);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 30, 50);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 60.1, 80.0);
            SetSkill(SkillName.MagicResist, 85.5, 100.0);
            SetSkill(SkillName.Tactics, 60.1, 80.0);
            SetSkill(SkillName.Wrestling, 50.1, 70.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 50;

            m_NextStormTime = DateTime.Now + m_StormDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextStormTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 12))
                {
                    this.Say("Feel the fury of the storm!");
                    Effects.PlaySound(Location, Map, 0x28E); // Storm sound effect

                    foreach (Mobile m in this.GetMobilesInRange(8))
                    {
                        if (m != this && this.CanBeHarmful(m) && m.Combatant == this)
                        {
                            this.DoHarmful(m);
                            m.Damage(Utility.RandomMinMax(20, 30), this);
                            m.SendMessage("You are struck by a lightning bolt!");
                        }
                    }

                    m_NextStormTime = DateTime.Now + m_StormDelay;
                }
            }

            base.OnThink();
        }

        public override void GenerateLoot()
        {
            PackGold(200, 300);
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 15)));
            PackItem(new BlackPearl(Utility.RandomMinMax(5, 15)));
        }

        public BattleStormCaller(Serial serial) : base(serial)
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
