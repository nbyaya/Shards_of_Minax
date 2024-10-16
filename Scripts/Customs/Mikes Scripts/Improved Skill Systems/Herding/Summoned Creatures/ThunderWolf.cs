using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a thunder wolf corpse")]
    public class ThunderWolf : BaseCreature
    {
        private DateTime m_NextThunderStrike;

        [Constructable]
        public ThunderWolf()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a thunder wolf";
            Body = 225;
            BaseSoundID = 0xE5;
            Hue = 1152; // Electric blue hue

            this.SetStr(180);
            this.SetDex(120);
            this.SetInt(100);

            this.SetHits(150);
            this.SetMana(100);

            this.SetDamage(12, 18);

            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Energy, 50);

            this.SetResistance(ResistanceType.Physical, 40, 50);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Cold, 30, 40);
            this.SetResistance(ResistanceType.Poison, 50, 60);
            this.SetResistance(ResistanceType.Energy, 60, 70);

            this.SetSkill(SkillName.MagicResist, 70.1, 90.0);
            this.SetSkill(SkillName.Tactics, 80.0, 100.0);
            this.SetSkill(SkillName.Wrestling, 80.0, 100.0);

            this.VirtualArmor = 50;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -10;

            m_NextThunderStrike = DateTime.UtcNow;
        }

        public ThunderWolf(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 2; } }
        public override int Hides { get { return 12; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextThunderStrike)
            {
                ThunderStrike();
            }
        }

        private void ThunderStrike()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                target.SendMessage("You are struck by a thunderous blow!");
                target.PlaySound(0x29);
                target.FixedEffect(0x3779, 10, 16);
                target.Paralyze(TimeSpan.FromSeconds(2.0)); // Stuns for 2 seconds

                target.AddStatMod(new StatMod(StatType.Str, "ThunderStrikeStr", -10, TimeSpan.FromSeconds(30)));
                target.AddStatMod(new StatMod(StatType.Dex, "ThunderStrikeDex", -10, TimeSpan.FromSeconds(30)));
                target.AddStatMod(new StatMod(StatType.Int, "ThunderStrikeInt", -10, TimeSpan.FromSeconds(30)));

                m_NextThunderStrike = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown of 45 seconds
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write((DateTime)m_NextThunderStrike);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextThunderStrike = reader.ReadDateTime();
        }
    }
}
