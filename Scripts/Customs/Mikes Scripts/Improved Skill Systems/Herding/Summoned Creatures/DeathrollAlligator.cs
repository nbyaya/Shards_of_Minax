using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a deathroll alligator corpse")]
    public class DeathrollAlligator : BaseCreature
    {
        private DateTime m_NextDeathroll;

        [Constructable]
        public DeathrollAlligator()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a deathroll alligator";
            Body = 0xCA;
            BaseSoundID = 660;
            Hue = 1175; // Unique hue

            this.SetStr(250);
            this.SetDex(100);
            this.SetInt(50);

            this.SetHits(300);
            this.SetStam(200);
            this.SetMana(0);

            this.SetDamage(20, 30);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 50, 60);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Poison, 40, 50);

            this.SetSkill(SkillName.MagicResist, 80.0);
            this.SetSkill(SkillName.Tactics, 90.0);
            this.SetSkill(SkillName.Wrestling, 90.0);

            this.Fame = 1500;
            this.Karma = -1500;

            this.VirtualArmor = 50;

            this.Tamable = true;
            this.ControlSlots = 1;
            this.MinTameSkill = -10;

            m_NextDeathroll = DateTime.UtcNow;
        }

        public DeathrollAlligator(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 12; } }
        public override HideType HideType { get { return HideType.Spined; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextDeathroll)
            {
                PerformDeathroll();
            }
        }

        private void PerformDeathroll()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Deathroll! *");
                PlaySound(0x1F1);
                FixedEffect(0x3728, 10, 20);

                int damage = Utility.RandomMinMax(40, 60);
                target.Damage(damage, this);
                target.Paralyze(TimeSpan.FromSeconds(5));

                m_NextDeathroll = DateTime.UtcNow + TimeSpan.FromMinutes(2);
            }
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

            m_NextDeathroll = DateTime.UtcNow;
        }
    }
}
