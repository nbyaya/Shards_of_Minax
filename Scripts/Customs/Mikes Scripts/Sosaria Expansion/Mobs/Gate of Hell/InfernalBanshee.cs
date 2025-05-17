using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;            // For potential spell effects
using Server.Network;           // For effect messaging
using System.Collections.Generic; // For AoE targeting
using Server.Spells.Fourth;     // For fire field effects

namespace Server.Mobiles
{
    [CorpseName("an infernal banshee corpse")]
    public class InfernalBanshee : BaseCreature
    {
        // --- Ability cooldown timers ---
        private DateTime m_NextNovaTime;
        private DateTime m_NextMeteorTime;
        private Point3D m_LastLocation;

        // --- Unique fiery hue; adjust as desired ---
        private const int UniqueHue = 1161;

        [Constructable]
        public InfernalBanshee()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "an Infernal Banshee";
            Body = 310; // Same as the wailing banshee body
            BaseSoundID = 0x482; // Same as standard banshee sound
            Hue = UniqueHue;


            // --- Advanced Stats ---
            SetStr(400, 500);
            SetDex(250, 300);
            SetInt(400, 500);

            SetHits(1300, 1500); // Substantial health for boss-level threat
            SetStam(250, 300);
            SetMana(400, 500);

            SetDamage(18, 24); // Base melee damage

            // Damage types: mix physical with a heavy fire focus
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 80);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 90, 100);
            SetResistance(ResistanceType.Cold, -10, 0);  // Vulnerable to cold
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            // --- Skills (advanced magic & combat) ---
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 110.0, 125.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 80;
            ControlSlots = 5;

            // --- Initialize ability cooldowns ---
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_LastLocation = this.Location;

            // --- Base Loot and thematic extra drops ---
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 8)));
        }

        // --- Unique Ability: Scorched Aura on Movement ---
        // As targets move within 2 tiles, leave behind a spectral fire field that damages them.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m == null || m == this || m.Map != this.Map || !m.Alive || !this.Alive)
            {
                base.OnMovement(m, oldLocation);
                return;
            }

            if (m.InRange(this.Location, 2))
            {
                // Leave behind a fire field at the mobile's old location
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(4));
                int damage = 4; // Slightly increased damage for the infernal effect

                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                // Play a spectral, wailing effect coupled with fire
                m.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);
                m.PlaySound(0x208);

                // Damage the mobile (fire-only)
                if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(8, 16), 0, 100, 0, 0, 0); // 100% fire damage
                }
            }

            base.OnMovement(m, oldLocation);
        }

        // --- Ability Thinker ---
        public override void OnThink()
        {
            base.OnThink();

            // Leave a scorched field at the old location when moving
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;

                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(8 + Utility.Random(5));
                int damage = 4;

                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);
            }

            // Check for combat and abilities usage
            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Hellfire Nova: AoE attack if target within 8 tiles
            if (DateTime.UtcNow >= m_NextNovaTime && this.InRange(Combatant.Location, 8))
            {
                HellfireNovaAttack();
                m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            }
            // Searing Meteor Swarm: Ranged AoE attack if target within 12 tiles
            else if (DateTime.UtcNow >= m_NextMeteorTime && this.InRange(Combatant.Location, 12))
            {
                SearingMeteorSwarmAttack();
                m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        // --- Unique Ability: Hellfire Nova ---
        // A powerful, blast-radius attack that emits a searing wail and scorching flames.
        public void HellfireNovaAttack()
        {
            if (Map == null)
                return;

            // Visual and sound effects: a wail accompanied by fiery explosion effects
            this.PlaySound(0x208);
            this.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.LeftFoot);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                // A burst effect radiates from the banshee
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 
                    0x3709, 10, 60, UniqueHue, 0, 5029, 0);

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(50, 70);
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0); // Fire-only damage

                    // Fiery impact visuals on target
                    target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);
                }
            }
        }

        // --- Unique Ability: Searing Meteor Swarm ---
        // Calls down several blazing meteors onto the area around its primary target.
        public void SearingMeteorSwarmAttack()
        {
            if (Combatant == null || Map == null)
                return;

            if (!(Combatant is Mobile target))
                return;

            Point3D targetLocation = target.Location;
            this.Say("*The banshee unleashes a wailing inferno!*");
            PlaySound(0x160);

            int meteorCount = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < meteorCount; i++)
            {
                Point3D impactPoint = new Point3D(
                    targetLocation.X + Utility.RandomMinMax(-3, 3),
                    targetLocation.Y + Utility.RandomMinMax(-3, 3),
                    targetLocation.Z);

                if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                {
                    impactPoint.Z = Map.GetAverageZ(impactPoint.X, impactPoint.Y);
                    if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                        continue;
                }

                // Launch meteor effect: start high and send moving particles to the impact point.
                Point3D startPoint = new Point3D(impactPoint.X + Utility.RandomMinMax(-1, 1), impactPoint.Y + Utility.RandomMinMax(-1, 1), impactPoint.Z + 30);
                Effects.SendMovingParticles(new Entity(Serial.Zero, startPoint, this.Map), 
                    new Entity(Serial.Zero, impactPoint, this.Map),
                    0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                // Stagger the meteor impacts
                Timer.DelayCall(TimeSpan.FromSeconds(0.5 + (i * 0.1)), () =>
                {
                    if (this.Map == null)
                        return;

                    // Impact visual effect
                    Effects.SendLocationParticles(EffectItem.Create(impactPoint, this.Map, EffectItem.DefaultDuration), 
                        0x3709, 10, 30, UniqueHue, 0, 2023, 0);

                    IPooledEnumerable affected = Map.GetMobilesInRange(impactPoint, 1);
                    foreach (Mobile m in affected)
                    {
                        if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                        {
                            DoHarmful(m);
                            int damage = Utility.RandomMinMax(25, 35);
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0); // Fire-only damage
                        }
                    }
                    affected.Free();
                });
            }
        }

        // --- Death Explosion: Fiery Aftermath ---
        // On death the banshee scatters burning ground (using HotLavaTile) and produces a massive explosion.
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int lavaTilesToDrop = 10;
            List<Point3D> effectLocations = new List<Point3D>();

            for (int i = 0; i < lavaTilesToDrop; i++)
            {
                int xOffset = Utility.RandomMinMax(-3, 3);
                int yOffset = Utility.RandomMinMax(-3, 3);
                if (xOffset == 0 && yOffset == 0 && i < lavaTilesToDrop - 1)
                    xOffset = Utility.RandomBool() ? 1 : -1;

                Point3D lavaLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);
                if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                {
                    lavaLocation.Z = Map.GetAverageZ(lavaLocation.X, lavaLocation.Y);
                    if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                        continue;
                }

                effectLocations.Add(lavaLocation);

                // Spawn a HotLavaTile with matching hue; assumes HotLavaTile is defined elsewhere.
                HotLavaTile droppedLava = new HotLavaTile();
                droppedLava.Hue = UniqueHue;
                droppedLava.MoveToWorld(lavaLocation, this.Map);

                Effects.SendLocationParticles(EffectItem.Create(lavaLocation, this.Map, EffectItem.DefaultDuration), 
                    0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            // Main explosion effect at one of the generated points
            Point3D deathLocation = this.Location;
            if (effectLocations.Count > 0)
                deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathLocation, this.Map, 0x218);
            Effects.SendLocationParticles(EffectItem.Create(deathLocation, this.Map, EffectItem.DefaultDuration), 
                0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override double DispelDifficulty { get { return 135.0; } }
        public override double DispelFocus { get { return 60.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 1);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));

            // Chance for a rare thematic drop
            if (Utility.RandomDouble() < 0.01)
                PackItem(new InfernosEmbraceCloak());
            if (Utility.RandomDouble() < 0.05)
                PackItem(new DaemonBone(Utility.RandomMinMax(3, 5)));
        }

        // --- Serialization ---
        public InfernalBanshee(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
        }
    }
}
