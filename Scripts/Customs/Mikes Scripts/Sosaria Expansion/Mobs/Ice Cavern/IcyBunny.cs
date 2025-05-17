using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("an icy bunny corpse")]
    public class IcyBunny : BaseCreature
    {
        private DateTime m_NextFrostNova;
        private DateTime m_NextBlink;
        private DateTime m_NextSnowstorm;
        private DateTime m_NextShatterHop;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public IcyBunny()
            : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.1, 0.2)
        {
            Name = "an icy bunny";
            Body = 205;
            Hue = 0x47E; // Frosty cyan
            BaseSoundID = 0xC9;

            SetStr(500);
            SetDex(250);
            SetInt(1000);

            SetHits(2500);
            SetStam(1000);
            SetMana(2000);

            SetDamage(12, 18);

            SetDamageType(ResistanceType.Cold, 80);
            SetDamageType(ResistanceType.Physical, 20);

            SetResistance(ResistanceType.Physical, 50);
            SetResistance(ResistanceType.Fire, 10);
            SetResistance(ResistanceType.Cold, 85);
            SetResistance(ResistanceType.Poison, 40);
            SetResistance(ResistanceType.Energy, 30);

            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 90.0);
            SetSkill(SkillName.Wrestling, 85.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.EvalInt, 100.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 60;

            DelayBeginTunnel();
            m_AbilitiesInitialized = false;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && !m_AbilitiesInitialized)
            {
                m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(10);
                m_NextBlink = DateTime.UtcNow + TimeSpan.FromSeconds(15);
                m_NextSnowstorm = DateTime.UtcNow + TimeSpan.FromSeconds(25);
                m_NextShatterHop = DateTime.UtcNow + TimeSpan.FromSeconds(45);
                m_AbilitiesInitialized = true;
            }

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextFrostNova)
                    CastFrostNova();

                if (DateTime.UtcNow >= m_NextBlink)
                    BlinkBehindTarget();

                if (DateTime.UtcNow >= m_NextSnowstorm)
                    CastSnowstorm();

                if (DateTime.UtcNow >= m_NextShatterHop)
                    ShatterHop();
            }
        }

        private void CastFrostNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* An icy nova erupts from the bunny! *");
            PlaySound(0x64E);
            Effects.PlaySound(Location, Map, 0x64E);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, TimeSpan.FromSeconds(0.5)), 0x375A, 16, 20, Hue, 0, 5029, 0);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 100, 0, 0);
                    m.Freeze(TimeSpan.FromSeconds(2.0));
                    m.SendMessage("You are chilled to the bone by the frost nova!");
                }
            }

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        private void BlinkBehindTarget()
        {
            if (Combatant != null && Combatant is Mobile target && !target.Deleted)
            {
                Point3D newLoc = target.Location;
                newLoc = new Point3D(newLoc.X - 1, newLoc.Y - 1, newLoc.Z);
                MoveToWorld(newLoc, Map);
                Say("* The bunny vanishes and reappears in a flurry of frost! *");
                Effects.SendLocationEffect(Location, Map, 0x10B3, 30);
                PlaySound(0x665);

                if (target.Alive)
                {
                    AOS.Damage(target, this, 10, 0, 0, 100, 0, 0);
                    target.SendMessage("The icy bunny strikes as it blinks behind you!");
                }

                m_NextBlink = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }
        }

        private void CastSnowstorm()
        {
            Say("* A swirling snowstorm begins to form... *");
            PlaySound(0x64E);
            Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
            {
                Effects.SendMovingEffect(this, Combatant, 0x36BD, 10, 1, false, false);
                if (Combatant is Mobile target)
                {
                    AOS.Damage(target, this, Utility.RandomMinMax(25, 35), 0, 0, 100, 0, 0);
                    target.Freeze(TimeSpan.FromSeconds(1));
                    target.SendMessage("A sudden snowstorm lashes at your skin!");
                }
            });

            m_NextSnowstorm = DateTime.UtcNow + TimeSpan.FromSeconds(35);
        }

        private void ShatterHop()
        {
            Say("* The Icy Bunny stomps and the ground cracks! *");
            PlaySound(0x207);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && m is Mobile mobile)
                {
                    mobile.SendMessage("The ground beneath you shatters!");
                    AOS.Damage(mobile, this, Utility.RandomMinMax(18, 30), 0, 0, 100, 0, 0);
                    mobile.Freeze(TimeSpan.FromSeconds(1.5));
                }
            }

            m_NextShatterHop = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Rich, 3);

            if (Utility.RandomDouble() < 0.01) // Rare drop
                PackItem(new IceCrystalRelic());
        }

        public override int Meat => 2;
        public override int Hides => 2;
        public override bool BardImmune => true;

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (Utility.RandomDouble() < 0.1)
            {
                Say("* The icy bunny flickers out of phase! *");
                Hidden = true;
                Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
                {
                    Hidden = false;
                    Say("* The icy bunny flickers back into view! *");
                });
            }
        }

        public virtual void DelayBeginTunnel()
        {
            Timer.DelayCall(TimeSpan.FromMinutes(3.0), BeginTunnel);
        }

        public virtual void BeginTunnel()
        {
            if (Deleted)
                return;

            new LandmineTile().MoveToWorld(Location, Map);
            Frozen = true;
            Say("* The bunny begins to dig a tunnel into the frozen depths... *");
            PlaySound(0x247);

            Timer.DelayCall(TimeSpan.FromSeconds(5.0), Delete);
        }

        public override int GetAttackSound() => 0xC9;
        public override int GetHurtSound() => 0xCA;
        public override int GetDeathSound() => 0xCB;

        public IcyBunny(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            DelayBeginTunnel();
            m_AbilitiesInitialized = false;
        }

        public class IceCrystalRelic : Item
        {
            public IceCrystalRelic() : base(0x1F19)
            {
                Name = "a shard of frost-infused crystal";
                Hue = 0x47E;
                Weight = 1.0;
            }

            public IceCrystalRelic(Serial serial) : base(serial) { }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write(0);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }
        }
    }
}
