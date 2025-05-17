using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a dire revenant corpse")]
    public class DireRevenant : BaseCreature
    {
        private DateTime m_NextSoulLeech;
        private DateTime m_NextWailingStrike;
        private DateTime m_NextPhantomBlink;
        private DateTime m_NextTerrorPulse;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public DireRevenant()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Dire Revenant";
            Body = 0x592;
            Hue = 2101; // A ghostly bluish/purple hue
            BaseSoundID = 0x3EF;

            SetStr(850, 950);
            SetDex(200, 240);
            SetInt(300, 400);

            SetHits(1000, 1200);
            SetDamage(25, 30);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 70, 85);
            SetResistance(ResistanceType.Energy, 55, 70);

            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 100.0);
            SetSkill(SkillName.Necromancy, 80.0, 100.0);

            Fame = 26000;
            Karma = -26000;

            VirtualArmor = 85;

            Tamable = false;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null)
                return;

            if (!m_AbilitiesInitialized)
            {
                var rand = new Random();
                m_NextSoulLeech = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 15));
                m_NextWailingStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 25));
                m_NextPhantomBlink = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 40));
                m_NextTerrorPulse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 50));
                m_AbilitiesInitialized = true;
            }

            if (DateTime.UtcNow >= m_NextSoulLeech)
                SoulLeech();

            if (DateTime.UtcNow >= m_NextWailingStrike)
                WailingStrike();

            if (DateTime.UtcNow >= m_NextPhantomBlink)
                PhantomBlink();

            if (DateTime.UtcNow >= m_NextTerrorPulse)
                TerrorPulse();
        }

        private void SoulLeech()
        {
            if (Combatant is Mobile target && target.Alive)
            {
                Say("*The Dire Revenant drains your soul!*");
                target.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Head);
                PlaySound(0x1F8);

                int damage = Utility.RandomMinMax(15, 30);
                AOS.Damage(target, this, damage, 0, 0, 100, 0, 0);

                Heal(damage); // Revenant heals from the stolen life
            }

            m_NextSoulLeech = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void WailingStrike()
        {
            if (Combatant is Mobile target && target.Alive)
            {
                Say("*A wailing shriek pierces your mind!*");
                target.PlaySound(0x56E); // Wail
                target.FixedEffect(0x37C4, 10, 20);

                target.Stam -= Utility.RandomMinMax(10, 20);
                target.Mana -= Utility.RandomMinMax(10, 20);
                target.SendMessage("You reel from the Revenant's psychic strike!");

                AOS.Damage(target, this, Utility.RandomMinMax(10, 20), 0, 0, 50, 50, 0);
            }

            m_NextWailingStrike = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        private void PhantomBlink()
        {
            Say("*The Revenant fades... and reappears!*");
            PlaySound(0x204); // Whoosh sound

            Point3D targetLocation = GetRandomLocationNearby();
            Map map = this.Map;

            if (map != null && map.CanSpawnMobile(targetLocation.X, targetLocation.Y, targetLocation.Z))
            {
                Effects.SendLocationEffect(Location, Map, 0x3728, 10);
                MoveToWorld(targetLocation, map);
                Effects.SendLocationEffect(Location, Map, 0x3728, 10);
            }

            m_NextPhantomBlink = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private Point3D GetRandomLocationNearby()
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Map.GetAverageZ(x, y);

                if (Map.CanSpawnMobile(x, y, z))
                    return new Point3D(x, y, z);
            }
            return Location;
        }

        private void TerrorPulse()
        {
            Say("*A pulse of pure dread erupts from the Revenant!*");
            PlaySound(0x22F); // Magical burst
            Effects.SendLocationEffect(Location, Map, 0x3709, 15, 10);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m == this || !m.Alive || !CanBeHarmful(m)) continue;

                if (m is Mobile target)
                {
                    m.FixedParticles(0x374A, 10, 20, 5032, EffectLayer.Waist);
                    m.SendMessage("You are overwhelmed by dread!");
                    m.Paralyze(TimeSpan.FromSeconds(2));
                    m.Stam -= Utility.RandomMinMax(5, 10);
                }
            }

            m_NextTerrorPulse = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.01) // Rare drop
                PackItem(new RevenantVeil()); // Custom rare item
        }

        public override bool AutoDispel => true;
        public override bool ReacquireOnMovement => true;
        public override bool CanAngerOnTame => false;
        public override bool StatLossAfterTame => false;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int GetIdleSound() { return 0x673; }
        public override int GetAngerSound() { return 0x670; }
        public override int GetHurtSound() { return 0x672; }
        public override int GetDeathSound() { return 0x671; }

        public DireRevenant(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    public class RevenantVeil : Cloak
    {
        [Constructable]
        public RevenantVeil() : base()
        {
            Name = "Veil of the Dire Revenant";
            Hue = 2101;
            Attributes.BonusMana = 10;
            Attributes.CastRecovery = 2;
            Attributes.LowerRegCost = 20;
            LootType = LootType.Blessed;
        }

        public RevenantVeil(Serial serial) : base(serial) { }

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
