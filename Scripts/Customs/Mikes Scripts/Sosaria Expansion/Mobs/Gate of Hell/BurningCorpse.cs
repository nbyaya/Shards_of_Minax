using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;         // For any additional spell effects
using Server.Network;        // For particle/sound effects
using System.Collections.Generic;
using Server.Spells.Fourth;  // For fire field effects

namespace Server.Mobiles
{
    [CorpseName("a blazing corpse")]
    public class BurningCorpse : BaseCreature
    {
        // --- Ability Cooldowns & Tracking ---
        private DateTime m_NextAuraTime;
        private DateTime m_NextNovaTime;
        private DateTime m_NextSurgeTime;
        private Point3D m_LastLocation;

        // --- Unique Hue for this advanced fire creature ---
        private const int UniqueHue = 1250;

        [Constructable]
        public BurningCorpse() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "a burning corpse";
            Body = 153;             // Based on Ghoul body
            BaseSoundID = 0x482;      // Based on Ghoul sounds
            Hue = UniqueHue;

            // --- Advanced Stats ---
            SetStr(400, 500);
            SetDex(150, 200);
            SetInt(200, 250);

            SetHits(800, 1000);
            SetStam(150, 200);
            SetMana(200, 250);

            SetDamage(20, 30);
            // Damage is split: a little physical, but mostly fire damage
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Fire, 90);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 80, 90); // Highly resistant to fire
            SetResistance(ResistanceType.Cold, -10, 5);  // Vulnerable to cold
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            // --- Enhanced Skills ---
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 100.2, 110.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);
            SetSkill(SkillName.Anatomy, 80.0, 90.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 50;
            ControlSlots = 3;

            // --- Initialize Ability Timers & Location Tracking ---
            m_NextAuraTime = DateTime.UtcNow;
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextSurgeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_LastLocation = this.Location;

            // --- Base Loot ---
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(3, 6)));
        }

        // --- Burning Aura: Leaves fiery trails on movement ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            // If a nearby mobile moves within 2 tiles, leave a burning trail at its previous location
            if (m != null && m != this && m.Map == this.Map && m.Alive && this.Alive)
            {
                if (this.InRange(m.Location, 2))
                {
                    int itemID = 0x398C;
                    TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(4));
                    int damage = 3;
                    
                    var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                    // Visual and audio effects for the burning aura
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                    m.PlaySound(0x208);
                    
                    if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0); // 100% fire damage
                    }
                }
            }
            base.OnMovement(m, oldLocation);
        }

        // --- AI Think: Handle special attacks based on cooldowns ---
        public override void OnThink()
        {
            base.OnThink();

            // If the creature has moved, leave a trail effect behind at its old location
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(8 + Utility.Random(5));
                int damage = 3;
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);
            }

            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Before accessing Mobile-specific properties, ensure Combatant is a Mobile:
            if (!(Combatant is Mobile target))
                return;

            // --- Flame Nova Attack ---
            if (DateTime.UtcNow >= m_NextNovaTime && this.InRange(target.Location, 6))
            {
                FlameNovaAttack();
                m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // --- Fire Surge Attack: A targeted ranged burst ---
            else if (DateTime.UtcNow >= m_NextSurgeTime && this.InRange(target.Location, 12))
            {
                FireSurgeAttack();
                m_NextSurgeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
        }

        // --- Flame Nova: An explosive burst damaging all nearby targets ---
        public void FlameNovaAttack()
        {
            if (Map == null)
                return;

            PlaySound(0x208);
            FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

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
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5029, 0);
                foreach (Mobile m in targets)
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(40, 60); // AoE damage
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
                }
            }
        }

        // --- Fire Surge: A ranged fire projectile against a target ---
        public void FireSurgeAttack()
        {
            if (Map == null || Combatant == null)
                return;

            if (!(Combatant is Mobile target))
                return;

            // Flavor text and sound for the surge attack
            this.Say("*Incinerating surge!*");
            PlaySound(0x160);

            Point3D targetLocation = target.Location;
            Point3D startPoint = new Point3D(this.Location.X, this.Location.Y, this.Location.Z + 10);

            // Launch a moving particle effect from this creature to the target
            Effects.SendMovingParticles(new Entity(Serial.Zero, startPoint, this.Map), new Entity(Serial.Zero, targetLocation, this.Map),
                0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

            // Delay impact to match projectile travel
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null)
                    return;

                Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 2023, 0);
                if (CanBeHarmful(target, false) && SpellHelper.ValidIndirectTarget(this, target))
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(50, 70);
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
                }
            });
        }

        // --- OnDeath: Fiery explosion leaving behind burning embers ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                int emberCount = 8;
                List<Point3D> effectLocations = new List<Point3D>();

                for (int i = 0; i < emberCount; i++)
                {
                    int xOffset = Utility.RandomMinMax(-3, 3);
                    int yOffset = Utility.RandomMinMax(-3, 3);
                    if (xOffset == 0 && yOffset == 0 && i < emberCount - 1)
                        xOffset = Utility.RandomBool() ? 1 : -1;

                    Point3D emberLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);
                    if (!Map.CanFit(emberLocation.X, emberLocation.Y, emberLocation.Z, 16, false, false))
                    {
                        emberLocation.Z = Map.GetAverageZ(emberLocation.X, emberLocation.Y);
                        if (!Map.CanFit(emberLocation.X, emberLocation.Y, emberLocation.Z, 16, false, false))
                            continue;
                    }

                    effectLocations.Add(emberLocation);

                    // Spawn a HotLavaTile (assumed to be defined elsewhere) at this location
                    HotLavaTile droppedLava = new HotLavaTile();
                    droppedLava.Hue = UniqueHue;
                    droppedLava.MoveToWorld(emberLocation, this.Map);

                    Effects.SendLocationParticles(EffectItem.Create(emberLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 20, UniqueHue, 0, 5016, 0);
                }

                Point3D centralLocation = this.Location;
                if (effectLocations.Count > 0)
                    centralLocation = effectLocations[Utility.Random(effectLocations.Count)];

                Effects.PlaySound(centralLocation, this.Map, 0x218);
                Effects.SendLocationParticles(EffectItem.Create(centralLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);
            }

            base.OnDeath(c);
        }

        // --- Standard Overrides ---
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

            if (Utility.RandomDouble() < 0.01)
            {
                PackItem(new InfernosEmbraceCloak());
            }
            if (Utility.RandomDouble() < 0.05)
            {
                PackItem(new DaemonBone(Utility.RandomMinMax(3, 5)));
            }
        }

        public BurningCorpse(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version number
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reinitialize cooldown timers on load
            m_NextAuraTime = DateTime.UtcNow;
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextSurgeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
        }
    }
}
