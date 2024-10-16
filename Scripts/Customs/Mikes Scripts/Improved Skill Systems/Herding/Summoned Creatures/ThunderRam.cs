using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a thunder ram corpse")]
    public class ThunderRam : BaseCreature
    {
        private DateTime m_NextThunderStrike;
        private DateTime m_NextCharge;

        [Constructable]
        public ThunderRam()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a thunder ram";
            Body = 0xE7; // Goat body
            BaseSoundID = 0x99;
            Hue = 1153; // Lightning blue hue

            this.SetStr(250);
            this.SetDex(120);
            this.SetInt(60);

            this.SetHits(200);
            this.SetMana(0);

            this.SetDamage(20, 30);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 50, 60);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Cold, 40, 50);
            this.SetResistance(ResistanceType.Poison, 20, 30);
            this.SetResistance(ResistanceType.Energy, 50, 60);

            this.SetSkill(SkillName.MagicResist, 75.0, 85.0);
            this.SetSkill(SkillName.Tactics, 85.0, 95.0);
            this.SetSkill(SkillName.Wrestling, 85.0, 95.0);

            this.Fame = 2500;
            this.Karma = 2500;

            this.VirtualArmor = 60;

            this.Tamable = true;
            this.ControlSlots = 1;
            this.MinTameSkill = -10;

            m_NextThunderStrike = DateTime.UtcNow;
            m_NextCharge = DateTime.UtcNow;
        }

        public ThunderRam(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextThunderStrike)
                {
                    ThunderStrike();
                }

                if (DateTime.UtcNow >= m_NextCharge)
                {
                    Charge();
                }
            }
        }

        private void ThunderStrike()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(10, 20);
                target.Damage(damage, this);
                target.Paralyze(TimeSpan.FromSeconds(2.0));
                target.PlaySound(0x29);
                target.FixedEffect(0x37B9, 10, 16);
                m_NextThunderStrike = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }
        }

        private void Charge()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*charges*");
                PlaySound(0x340);
                FixedParticles(0x3728, 1, 13, 9912, 37, 7, EffectLayer.Head);
                int damage = Utility.RandomMinMax(15, 25);
                target.Damage(damage, this);
                m_NextCharge = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        public override int Meat { get { return 2; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.GrainsAndHay | FoodType.FruitsAndVegies; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextThunderStrike = DateTime.UtcNow;
            m_NextCharge = DateTime.UtcNow;
        }
    }
}
