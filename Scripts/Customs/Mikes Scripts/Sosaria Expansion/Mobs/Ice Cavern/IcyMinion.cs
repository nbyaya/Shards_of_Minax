using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a corpse of icy minion")]
    public class FrostMinion : BaseCreature
    {
        private DateTime m_NextFrozenNova;
        private DateTime m_NextFrostPulse;
        private DateTime m_NextShatter;

        private static readonly int MaxWanderDistance = 40;

        [Constructable]
        public FrostMinion()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "an Icy Minion";
            Body = 9; // same as MinionOfScelestus
            Hue = 1152; // Light shimmering blue
            BaseSoundID = 357;

            SetStr(400, 450);
            SetDex(200, 250);
            SetInt(300, 350);

            SetHits(40000);
            SetStam(300, 400);
            SetMana(4000);

            SetDamage(20, 25);

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Cold, 80);
            SetDamageType(ResistanceType.Energy, 10);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 140.0, 160.0);
            SetSkill(SkillName.Tactics, 120.0, 130.0);
            SetSkill(SkillName.Wrestling, 120.0, 130.0);
            SetSkill(SkillName.Magery, 130.0, 140.0);
            SetSkill(SkillName.EvalInt, 125.0, 135.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 90;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Deleted)
                return;

            if (!Alive)
                return;

            if (DateTime.UtcNow >= m_NextFrozenNova)
                FrozenNova();

            if (DateTime.UtcNow >= m_NextFrostPulse)
                FrostPulse();

            if (DateTime.UtcNow >= m_NextShatter)
                ShatterStrike();
        }

        private void FrozenNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* The Icy Minion summons a Frozen Nova! *");
            PlaySound(0x10B); // Cold burst
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, TimeSpan.FromSeconds(0.5)), 0x3709, 10, 30, 1152, 2, 9912, 0);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m == this || !m.Alive || m.Blessed)
                    continue;

                if (m is Mobile target)
                {
                    AOS.Damage(target, this, Utility.RandomMinMax(30, 45), 0, 100, 0, 0, 0);
                    target.Freeze(TimeSpan.FromSeconds(3));
                    target.SendMessage("You are frozen in place by the Icy Minion's Nova!");
                }
            }

            m_NextFrozenNova = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void FrostPulse()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x481, true, "* The Icy Minion emits a pulse of draining frost... *");
                PlaySound(0x5C9); // Chilling hum

                AOS.Damage(target, this, Utility.RandomMinMax(20, 35), 0, 80, 20, 0, 0);

                target.Mana -= Utility.RandomMinMax(20, 40);
                target.Stam -= Utility.RandomMinMax(20, 40);
                target.SendMessage("Your energy drains under the icy pressure...");

                m_NextFrostPulse = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }
        }

        private void ShatterStrike()
        {
            if (Combatant is Mobile target && target.Frozen)
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "* The Icy Minion shatters its frozen enemy! *");
                Effects.SendTargetParticles(target, 0x374A, 10, 30, 1152, EffectLayer.CenterFeet);

                AOS.Damage(target, this, Utility.RandomMinMax(50, 75), 100, 0, 0, 0, 0);
                target.SendMessage("The freezing shell around you explodes!");

                m_NextShatter = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (defender is Mobile target && Utility.RandomDouble() < 0.2)
            {
                target.ApplyPoison(this, Poison.Greater);
                target.SendMessage("Frost seeps into your veins!");
                target.FixedEffect(0x376A, 9, 32);
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (0.001 > Utility.RandomDouble()) // 0.1%
            {
                c.DropItem(new FrozenHeartOfTheMinion());
            }
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.SuperBoss, 2);
            this.AddLoot(LootPack.UltraRich, 2);
            this.AddLoot(LootPack.Gems, 10);
        }

        public override int TreasureMapLevel => 5;
        public override Poison PoisonImmune => Poison.Lethal;
        public override bool BleedImmune => true;
        public override bool ReacquireOnMovement => true;
        public override bool AcquireOnApproach => true;
        public override int AcquireOnApproachRange => 12;


        public FrostMinion(Serial serial)
            : base(serial)
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

    public class FrozenHeartOfTheMinion : Item
    {
        [Constructable]
        public FrozenHeartOfTheMinion() : base(0x1C10)
        {
            Name = "Frozen Heart of the Minion";
            Hue = 1152;
            Weight = 1.0;
        }

        public FrozenHeartOfTheMinion(Serial serial) : base(serial) { }

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
