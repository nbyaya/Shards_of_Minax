using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("an abyssal horse corpse")]
    public class AbyssalHorse : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextChargeTime;
        private DateTime m_NextNeighTime;
        private DateTime m_NextAuraTime;
        private Point3D  m_LastLocation;

        // Unique dark hue for the Abyssal Horse
        private const int UniqueHue = 1175;

        [Constructable]
        public AbyssalHorse()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "an Abyssal Horse";
            Body = 0x90;             // same body as the sea horse
            BaseSoundID = 0xA8;      // horse-like sounds
            Hue = UniqueHue;

            // Stats
            SetStr(600, 700);
            SetDex(200, 250);
            SetInt(100, 150);

            SetHits(1800, 2100);
            SetStam(300, 350);
            SetMana(200, 250);

            SetDamage(25, 35);

            // Damage types: brutal physical plus chilling shadow
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Cold, 60);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            // Skills
            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);

            VirtualArmor = 90;
            Fame = 22000;
            Karma = -22000;
            ControlSlots = 6;

            // Cooldowns
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextNeighTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextAuraTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));

            m_LastLocation = this.Location;

            // Loot
            PackItem(new VoidCore());           // rare resource
            PackItem(new GhostOfTheFirstReaper());  // unique trophy banner
            AddLoot(LootPack.UltraRich, 2);
        }

        // Trail of shadowy vortexes as it moves
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Location != m_LastLocation && this.Map != null &&
                Utility.RandomDouble() < 0.20)
            {
                Point3D dropLoc = m_LastLocation;
                m_LastLocation = this.Location;

                if (!Map.CanFit(dropLoc.X, dropLoc.Y, dropLoc.Z, 16, false, false))
                    dropLoc.Z = Map.GetAverageZ(dropLoc.X, dropLoc.Y);

                var tile = new VortexTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(dropLoc, this.Map);
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Charge Attack: powerful rush
            if (DateTime.UtcNow >= m_NextChargeTime && InRange(Combatant.Location, 12))
            {
                ChargeAttack();
                m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Petrifying Neigh: stun and petrify in cone
            else if (DateTime.UtcNow >= m_NextNeighTime && InRange(Combatant.Location, 8))
            {
                PetrifyingNeigh();
                m_NextNeighTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            // Shadow Aura: periodic area cold damage + slow
            else if (DateTime.UtcNow >= m_NextAuraTime)
            {
                ShadowAura();
                m_NextAuraTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
        }

        // --- Unique Ability: Charge Attack ---
        private void ChargeAttack()
        {
            if (!(Combatant is Mobile target)) return;

            Say("*snorts with abyssal power!*");
            PlaySound(0x213); // heavy impact sound

            // Dash toward target
            Point3D dest = target.Location;
            MoveToWorld(dest, Map);

            // AoE trample around destination
            var affected = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(dest, 3))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    affected.Add(m);
            }

            foreach (var m in affected)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(60, 80), 100, 0, 0, 0, 0);
                m.Stam = Math.Max(0, m.Stam - Utility.RandomMinMax(20, 40));
                m.FixedParticles(0x3779, 10, 20, 5032, UniqueHue, 0, EffectLayer.Head);
            }
        }

        // --- Unique Ability: Petrifying Neigh ---
        private void PetrifyingNeigh()
        {
            Say("*a bone-chilling neigh echoes!*");
            PlaySound(0x2C9);

            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                // Cone in front
                foreach (var m in Map.GetMobilesInRange(this.Location, 6))
                {
                    if (m == this || !CanBeHarmful(m, false)) continue;
                    if (this.GetDirectionTo(m) != this.Direction) continue;

                    DoHarmful(m);
                    if (Utility.RandomDouble() < 0.30)
                    {
                        m.Freeze(TimeSpan.FromSeconds(Utility.RandomMinMax(3, 6)));
                        m.SendMessage("You feel your limbs turn to stone!");
                    }
                    AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 0, 0, 0, 0, 100);
                }
            }
        }

        // --- Unique Ability: Shadow Aura ---
        private void ShadowAura()
        {
            Say("*the darkness pulses!*");
            PlaySound(0x1FE);

            var list = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(this.Location, 5))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    list.Add(m);
            }

            foreach (var m in list)
            {
                DoHarmful(m);
                // Cold damage + slow
                AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 0, 0, 100);
                if (m is Mobile mob) mob.Poison = Poison.Lesser;
                m.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			
			if (Map == null) return;

            Say("*a final, haunted whinny echoes as it falls!*");
            PlaySound(0x211);

            // Spawn pools of poison around corpse
            for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var pool = new PoisonTile();
                pool.Hue = UniqueHue;
                pool.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // --- Loot & Properties ---
        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Deadly;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus     => 75.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));
            if (Utility.RandomDouble() < 0.03)
                PackItem(new ShadowleafWrap());  // rare mount trophy
        }

        public AbyssalHorse(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-sync timers
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextNeighTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextAuraTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_LastLocation   = this.Location;
        }
    }
}
