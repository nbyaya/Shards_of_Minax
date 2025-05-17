using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;
using System.Collections.Generic;
using Server.Spells.Fourth; // For FireFieldSpell effect (if applicable)

namespace Server.Mobiles
{
    [CorpseName("a charred firestone core")]
    public class FirestoneElemental : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextEruptionTime;
        private DateTime m_NextPillarTime;
        private Point3D m_LastLocation;

        // Unique fire-themed hue (adjust as desired)
        private const int UniqueHue = 1175;

        [Constructable]
        public FirestoneElemental() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "a Firestone Elemental";
            Body = 14;               // Based on the Earth Elemental body
            BaseSoundID = 838;       // Fire-themed sound from infernal elementals
            Hue = UniqueHue;

            // --- Enhanced Stats ---
            SetStr(500, 600);
            SetDex(220, 300);
            SetInt(400, 500);

            SetHits(1400, 1700);
            SetStam(250, 300);
            SetMana(400, 500);

            SetDamage(30, 40);
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Fire, 90);

            // --- Adjusted Resistances ---
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 90, 100);
            SetResistance(ResistanceType.Cold, -10, 0); // Vulnerable to cold
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            // --- Enhanced Skills ---
            SetSkill(SkillName.EvalInt, 100.1, 120.0);
            SetSkill(SkillName.Magery, 100.1, 120.0);
            SetSkill(SkillName.MagicResist, 110.1, 130.0);
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 100.1, 110.0);
            SetSkill(SkillName.Anatomy, 90.0, 100.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 80;
            ControlSlots = 5;
			Tamable = true;

            // Initialize ability cooldowns
            m_NextEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextPillarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_LastLocation = this.Location;

            // Standard loot items (fire/reagent themed loot can be added as desired)
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(3, 6)));

            // Light source to emphasize its fiery presence
            AddItem(new LightSource());
        }

        // --- Scorching Trail ---
        // As the elemental moves, it leaves behind a temporary fire field
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.Alive && this.Alive)
            {
                // Create a fire field effect at the old location
                int itemID = 0x398C; // Graphic representing fire field
                TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(4));
                int damage = 2;

                var field = new FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                // Show visual effects on the mobile that just moved
                m.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);
                m.PlaySound(0x208);
            }

            base.OnMovement(m, oldLocation);
        }

        // --- Unique Ability: Eruption Shockwave ---
        // An area attack that shakes the ground, damaging and burning nearby foes.
        public void EruptionShockwave()
        {
            if (Map == null)
                return;

            // Play a rumbling sound effect
            PlaySound(0x20F);
            FixedParticles(0x3709, 10, 40, 5042, UniqueHue, 0, EffectLayer.Waist);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                int damage = Utility.RandomMinMax(35, 50);
                // Apply fire-only damage
                AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
                target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);
            }
        }

        // --- Unique Ability: Lava Pillar Attack ---
        // Summons molten pillars near the primary target to strike with descending impact.
        public void LavaPillarAttack()
        {
            if (Combatant == null || Map == null)
                return;

            if (!(Combatant is Mobile))
                return;

            Mobile target = (Mobile)Combatant;
            if (!CanBeHarmful(target))
                return;

            Point3D targetLocation = target.Location;
            this.Say("*The Firestone Elemental summons a pillar of molten rock!*");
            PlaySound(0x160); // Meteor/impact sound effect

            int pillarCount = Utility.RandomMinMax(2, 4);
            for (int i = 0; i < pillarCount; i++)
            {
                // Choose a randomized impact point near the target
                Point3D impactPoint = new Point3D(
                    targetLocation.X + Utility.RandomMinMax(-2, 2),
                    targetLocation.Y + Utility.RandomMinMax(-2, 2),
                    targetLocation.Z);

                if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                {
                    impactPoint.Z = Map.GetAverageZ(impactPoint.X, impactPoint.Y);
                    if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                        continue;
                }

                // Visual effect: a descending lava pillar (using moving particles)
                Point3D startPoint = new Point3D(impactPoint.X, impactPoint.Y, impactPoint.Z + 30);
                Effects.SendMovingParticles(new Entity(Serial.Zero, startPoint, this.Map), new Entity(Serial.Zero, impactPoint, this.Map),
                    0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                // Apply damage after a short delay to simulate impact
                Timer.DelayCall(TimeSpan.FromSeconds(0.5 + (i * 0.1)), delegate
                {
                    if (Map == null)
                        return;
                    Effects.SendLocationParticles(EffectItem.Create(impactPoint, this.Map, EffectItem.DefaultDuration),
                        0x3709, 10, 30, UniqueHue, 0, 2023, 0);
                    
                    IPooledEnumerable affected = Map.GetMobilesInRange(impactPoint, 1);
                    foreach (Mobile m in affected)
                    {
                        if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                        {
                            DoHarmful(m);
                            int damage = Utility.RandomMinMax(25, 35);
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                        }
                    }
                    affected.Free();
                });
            }
        }

        // --- Passive Ability: Molten Armor ---
        // When hit in melee, the elemental burns its attacker.
        public override void OnGotMeleeAttack(Mobile attacker)
        {
            if (attacker != null && CanBeHarmful(attacker))
            {
                AOS.Damage(attacker, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                attacker.FixedParticles(0x3709, 10, 15, 5021, UniqueHue, 0, EffectLayer.Waist);
            }
            base.OnGotMeleeAttack(attacker);
        }

        // --- Main Thinking Logic ---
        public override void OnThink()
        {
            base.OnThink();

            // Leave a scorching trail at the previous location if moved
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(8 + Utility.Random(5));
                int damage = 2;
                var field = new FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);
            }

            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Ensure Combatant is a Mobile before accessing its properties
            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= m_NextEruptionTime && this.InRange(target.Location, 7))
                {
                    EruptionShockwave();
                    m_NextEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
                }
                else if (DateTime.UtcNow >= m_NextPillarTime && this.InRange(target.Location, 10))
                {
                    LavaPillarAttack();
                    m_NextPillarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                }
            }
        }

        // --- Death Explosion: Molten Debris ---
        // When the elemental dies, it spawns molten debris that cause a final fiery explosion.
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                int debrisCount = 10;
                List<Point3D> effectLocations = new List<Point3D>();

                for (int i = 0; i < debrisCount; i++)
                {
                    int xOffset = Utility.RandomMinMax(-3, 3);
                    int yOffset = Utility.RandomMinMax(-3, 3);
                    if (xOffset == 0 && yOffset == 0 && i < debrisCount - 1)
                        xOffset = Utility.RandomBool() ? 1 : -1;

                    Point3D debrisLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                    if (!Map.CanFit(debrisLocation.X, debrisLocation.Y, debrisLocation.Z, 16, false, false))
                    {
                        debrisLocation.Z = Map.GetAverageZ(debrisLocation.X, debrisLocation.Y);
                        if (!Map.CanFit(debrisLocation.X, debrisLocation.Y, debrisLocation.Z, 16, false, false))
                            continue;
                    }

                    effectLocations.Add(debrisLocation);

                    HotLavaTile lavaTile = new HotLavaTile();
                    lavaTile.Hue = UniqueHue;
                    lavaTile.MoveToWorld(debrisLocation, Map);
                    Effects.SendLocationParticles(EffectItem.Create(debrisLocation, Map, EffectItem.DefaultDuration),
                        0x3709, 10, 20, UniqueHue, 0, 5016, 0);
                }

                Point3D deathLocation = this.Location;
                if (effectLocations.Count > 0)
                    deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

                Effects.PlaySound(deathLocation, Map, 0x218);
                Effects.SendLocationParticles(EffectItem.Create(deathLocation, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0);
            }
            base.OnDeath(c);
        }

        // --- Standard Overrides & Loot ---
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
                PackItem(new InfernosEmbraceCloak());
            if (Utility.RandomDouble() < 0.05)
                PackItem(new DaemonBone(Utility.RandomMinMax(3, 5)));
        }

        public FirestoneElemental(Serial serial) : base(serial)
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

            m_NextEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextPillarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
        }
    }
}
