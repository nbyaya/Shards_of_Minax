using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;          // For potential spell effects
using Server.Network;         // For visual/sound effects
using Server.Spells.Fourth;   // For FireFieldSpell

namespace Server.Mobiles
{
    [CorpseName("a burning ravager corpse")]
    public class BurningRavager : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextFlameNovaTime;
        private DateTime m_NextChargeTime;
        private DateTime m_NextTrailTime;

        // Unique fire hue for the Burning Ravager (adjust as desired)
        private const int UniqueHue = 1161;

        [Constructable]
        public BurningRavager()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Burning Ravager";
            Body = 314;           // Use the same body as the original Ravager
            BaseSoundID = 357;    // Use the same sound set
            Hue = UniqueHue;      // Apply the unique fire tone

            // --- Boosted Stats ---
            SetStr(400, 450);
            SetDex(150, 200);
            SetInt(100, 140);

            SetHits(800, 900);

            SetStam(300, 350);
            SetMana(100, 150);

            SetDamage(20, 25);

            // Mix physical and fire damage; mostly fire
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Fire, 60);

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Upgraded Skills ---
            SetSkill(SkillName.MagicResist, 70.0, 85.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 80.0, 95.0);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 60;
            ControlSlots = 2;

            // Initialize ability cooldowns (in seconds)
            m_NextFlameNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextTrailTime = DateTime.UtcNow; // Ready immediately

            // Standard weapon abilities from the base creature
            SetWeaponAbility(WeaponAbility.CrushingBlow);
            SetWeaponAbility(WeaponAbility.Dismount);
        }

        // --- Movement-based Ability: Burning Trail ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.Alive && this.Alive)
            {
                if (DateTime.UtcNow >= m_NextTrailTime)
                {
                    // Create a burning trail at the mobile's previous location
                    int itemID = 0x398C; // Using an effect graphic for fire fields
                    TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(3));
                    int damage = 3; // Minor fire damage

                    var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                    // Visual and sound effects for the trail
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                    m.PlaySound(0x208);

                    // Set a short cooldown to avoid spamming
                    m_NextTrailTime = DateTime.UtcNow + TimeSpan.FromSeconds(2);
                }
            }
            base.OnMovement(m, oldLocation);
        }

        // --- Main Thinking Logic for Special Abilities ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Ensure Combatant is a Mobile before accessing its properties
            if (!(Combatant is Mobile target))
                return;

            // Use Flame Nova if the target is within 8 tiles and the cooldown has expired
            if (DateTime.UtcNow >= m_NextFlameNovaTime && this.InRange(target.Location, 8))
            {
                FlameNovaAttack();
                m_NextFlameNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            // Use Charge Attack if the target is very close (within 3 tiles)
            else if (DateTime.UtcNow >= m_NextChargeTime && this.InRange(target.Location, 3))
            {
                ChargeAttack(target);
                m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        // --- Unique Ability: Flame Nova Attack ---
        public void FlameNovaAttack()
        {
            if (Map == null)
                return;

            // Play fire sound and particles from self
            this.PlaySound(0x208);
            this.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

            // Gather targets within a 6-tile radius
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            // If there are targets, apply the AoE effect
            if (targets.Count > 0)
            {
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5029, 0);

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(30, 45);
                    // Deal pure fire damage
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
                    target.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
                }
            }
        }

        // --- Unique Ability: Charge Attack with Ignition ---
        public void ChargeAttack(Mobile target)
        {
            if (Map == null || target == null || !CanBeHarmful(target, false) || !SpellHelper.ValidIndirectTarget(this, target))
                return;

            // Announce and play charge sound effect
            this.Say("*The Burning Ravager charges, engulfing its foe in searing flames!*");
            this.PlaySound(0x56B);

            // Immediately damage the target
            DoHarmful(target);
            int damage = Utility.RandomMinMax(40, 60);
            AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
            target.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);

            // Simulate ignition with additional burning damage after a short delay
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                if (target != null && target.Alive)
                {
                    AOS.Damage(target, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                    target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                }
            });
        }

        // --- OnDeath: Fiery Explosion Effect ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int explosionTiles = 6; // Number of fiery patches to spawn
            List<Point3D> effectLocations = new List<Point3D>();

            for (int i = 0; i < explosionTiles; i++)
            {
                int xOffset = Utility.RandomMinMax(-2, 2);
                int yOffset = Utility.RandomMinMax(-2, 2);
                if (xOffset == 0 && yOffset == 0 && i < explosionTiles - 1)
                    xOffset = Utility.RandomBool() ? 1 : -1;

                Point3D explosionLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);
                if (!Map.CanFit(explosionLocation.X, explosionLocation.Y, explosionLocation.Z, 16, false, false))
                {
                    explosionLocation.Z = Map.GetAverageZ(explosionLocation.X, explosionLocation.Y);
                    if (!Map.CanFit(explosionLocation.X, explosionLocation.Y, explosionLocation.Z, 16, false, false))
                        continue;
                }
                effectLocations.Add(explosionLocation);

                // Spawn a HotLavaTile at each chosen location (assumes HotLavaTile is defined elsewhere)
                HotLavaTile lavaTile = new HotLavaTile();
                lavaTile.Hue = UniqueHue;
                lavaTile.MoveToWorld(explosionLocation, Map);
                Effects.SendLocationParticles(EffectItem.Create(explosionLocation, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            // Create a central explosion effect
            Point3D centralExplosion = this.Location;
            if (effectLocations.Count > 0)
                centralExplosion = effectLocations[Utility.Random(effectLocations.Count)];

			Effects.PlaySound(this.Location, this.Map, 0x218);
            Effects.SendLocationParticles(EffectItem.Create(centralExplosion, Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        // --- Standard Property Overrides ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override double DispelDifficulty { get { return 130.0; } }
        public override double DispelFocus { get { return 55.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls);

            // A small chance for a unique drop (assumes BurningRavagersEmbrace exists)
            if (Utility.RandomDouble() < 0.005)
                PackItem(new MaxxiaScroll());
        }

        public BurningRavager(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            // (Additional fields can be saved here if needed)
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reinitialize cooldown timers on load
            m_NextFlameNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextTrailTime = DateTime.UtcNow;
        }
    }
}
