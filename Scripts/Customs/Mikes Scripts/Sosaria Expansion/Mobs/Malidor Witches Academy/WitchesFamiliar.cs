using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // For any spell-based effects
using Server.Network; // For visual/sound effects
using System.Collections.Generic; // For list-based AoE targeting

namespace Server.Mobiles
{
    [CorpseName("a hexed familiar corpse")]
    public class WitchesFamiliar : BaseCreature
    {
        // Timers for the special abilities
        private DateTime m_NextChainTime;
        private DateTime m_NextRiftTime;
        private Point3D m_LastLocation;

        // Unique Hue for the Witches Familiar: a deep, arcane violet
        private const int UniqueHue = 0x4F6;

        [Constructable]
        public WitchesFamiliar() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "a Witches Familiar";
            Body = 0xC9; // Same body as Hell Cat
            BaseSoundID = 0x69; // Same base sound as Hell Cat
            Hue = UniqueHue; // Unique hue for this advanced magical creature

            // --- Stats: Advanced, magic-focused creature ---
            SetStr(250, 300);
            SetDex(300, 350);
            SetInt(300, 350);

            SetHits(100, 200);
            SetStam(300, 350);
            SetMana(400, 500);

            SetDamage(10, 15);
            // Damage: Part physical, but predominantly magical energy
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 80);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 80, 90);

            // --- Skills: Highly capable in magic ---
            SetSkill(SkillName.EvalInt, 100.1, 120.0);
            SetSkill(SkillName.Magery, 100.1, 120.0);
            SetSkill(SkillName.MagicResist, 105.2, 120.0);
            SetSkill(SkillName.Meditation, 90.0, 105.0);
            SetSkill(SkillName.Tactics, 70.1, 90.0);
            SetSkill(SkillName.Wrestling, 70.1, 90.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 40;

            // This creature is not intended to be tamable.
            Tamable = false;
            ControlSlots = 5;

            // Initialize ability cooldown timers
            m_NextChainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_LastLocation = this.Location;

            // Basic loot: reagents and spell scrolls
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 15)));
            PackItem(new Nightshade(Utility.RandomMinMax(10, 15)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(10, 15)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
        }

        // --- Hex Aura: Drains mana and delivers minor energy damage when foes draw near ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.Alive && InRange(this.Location, 2) && CanBeHarmful(m, false))
            {
                // Ensure the target is a Mobile before accessing specific properties
                if (m is Mobile targetMobile && SpellHelper.ValidIndirectTarget(this, targetMobile))
                {
                    DoHarmful(targetMobile);

                    // Drain mana and apply minor energy damage
                    int manaDrained = Utility.RandomMinMax(15, 25);
                    if (targetMobile.Mana >= manaDrained)
                    {
                        targetMobile.Mana -= manaDrained;
                        targetMobile.SendMessage(0x22, "A cursed aura drains your magical energy!");
                        targetMobile.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                        targetMobile.PlaySound(0x1F8);
                    }
                    // Inflict minor energy damage (100% energy)
                    AOS.Damage(targetMobile, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 0, 100);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        // --- OnThink: Check and trigger special abilities based on cooldown and proximity ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Prioritize special attacks based on cooldown and combat range
            if (DateTime.UtcNow >= m_NextChainTime && InRange(Combatant.Location, 10))
            {
                ChainHexAttack();
                m_NextChainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (DateTime.UtcNow >= m_NextRiftTime && InRange(Combatant.Location, 12))
            {
                CurseRiftAttack();
                m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }

            // Optional: Leave behind a trail of hazardous cursed gas as it moves
            if (this.Location != m_LastLocation && Map != null && Map != Map.Internal && Utility.RandomDouble() < 0.25)
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    // Spawning a ToxicGasTile to represent lingering cursed fumes
                    ToxicGasTile gas = new ToxicGasTile();
                    gas.Hue = UniqueHue;
                    gas.MoveToWorld(oldLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(oldLoc.X, oldLoc.Y);
                    if (Map.CanFit(oldLoc.X, oldLoc.Y, validZ, 16, false, false))
                    {
                        ToxicGasTile gas = new ToxicGasTile();
                        gas.Hue = UniqueHue;
                        gas.MoveToWorld(new Point3D(oldLoc.X, oldLoc.Y, validZ), this.Map);
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        // --- Unique Ability: Chain Hex Attack --- 
        // Bounces a cursed magical bolt between foes, dealing energy damage and sapping their vitality.
        public void ChainHexAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*The familiar unleashes a chain of hexes!*");
            PlaySound(0x20A); // Using an energy bolt sound effect

            List<Mobile> targets = new List<Mobile>();
            Mobile currentTarget = Combatant as Mobile;

            if (currentTarget == null || !CanBeHarmful(currentTarget, false) || !SpellHelper.ValidIndirectTarget(this, currentTarget))
                return;

            targets.Add(currentTarget);
            int maxTargets = 5;
            int range = 5;

            for (int i = 0; i < maxTargets; ++i)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDist = -1.0;

                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, range);
                foreach (Mobile m in eable)
                {
                    if (m != this && m != lastTarget && !targets.Contains(m) && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(lastTarget, m) && lastTarget.InLOS(m))
                    {
                        double dist = lastTarget.GetDistanceToSqrt(m);
                        if (closestDist == -1.0 || dist < closestDist)
                        {
                            closestDist = dist;
                            nextTarget = m;
                        }
                    }
                }
                eable.Free();

                if (nextTarget != null)
                    targets.Add(nextTarget);
                else
                    break;
            }

            // Apply damage and curse effect to each target in the chain
            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                // Visual effect: A bolt traveling from the source to the target
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                // Delay damage for visual sync
                Mobile damageTarget = target;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(damageTarget, false))
                    {
                        DoHarmful(damageTarget);
                        int damage = Utility.RandomMinMax(25, 35);
                        AOS.Damage(damageTarget, this, damage, 0, 0, 0, 0, 100);
                        damageTarget.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                        // Apply a cursed message effect (a visual cue for a hex)
                        damageTarget.SendMessage(0x22, "A lingering curse weakens your resolve!");
                    }
                });
            }
        }

        // --- Unique Ability: Curse Rift Attack ---
        // Tears open a small rift at the foe’s location, spawning a hazardous zone using a cursed flame tile.
        public void CurseRiftAttack()
        {
            if (Combatant == null || Map == null)
                return;

            Point3D targetLocation;
            IDamageable targetDamageable = Combatant;

            if (targetDamageable is Mobile targetMobile && CanBeHarmful(targetMobile, false))
                targetLocation = targetMobile.Location;
            else
                targetLocation = targetDamageable.Location;

            this.Say("*The fabric of reality shudders with cursed might!*");
            PlaySound(0x22F); // Rift-like sound

            // Display an initial burst effect at the target location
            Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5039, 0);

            // After a short delay, spawn a hazardous cursed rift
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null)
                    return;

                Point3D spawnLoc = targetLocation;
                if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                {
                    spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                    if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                        return;
                }

                // Use the NecromanticFlamestrikeTile to represent cursed, lingering magic
                NecromanticFlamestrikeTile riftTile = new NecromanticFlamestrikeTile();
                riftTile.Hue = UniqueHue;
                riftTile.MoveToWorld(spawnLoc, this.Map);

                Effects.PlaySound(spawnLoc, this.Map, 0x1F6);
            });
        }

        // --- Death Effect: Cursed Detonation ---
        // Upon death, the familiar releases a burst of cursed energy that spawns several hazardous tiles nearby.
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*The curse... is unleashed...*");
                PlaySound(0x211); // A magical explosion sound
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                // Spawn hazardous tiles around the corpse (using, for example, ToxicGasTile)
                int hazardsToDrop = Utility.RandomMinMax(4, 7);
                for (int i = 0; i < hazardsToDrop; i++)
                {
                    int xOffset = Utility.RandomMinMax(-4, 4);
                    int yOffset = Utility.RandomMinMax(-4, 4);
                    Point3D hazardLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                    if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                    {
                        hazardLocation.Z = Map.GetAverageZ(hazardLocation.X, hazardLocation.Y);
                        if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                            continue;
                    }

                    ToxicGasTile hazardTile = new ToxicGasTile();
                    hazardTile.Hue = UniqueHue;
                    hazardTile.MoveToWorld(hazardLocation, this.Map);
                    Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            // Generate loot similar to high-level magical entities
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            // Chance for a unique magical artifact drop
            if (Utility.RandomDouble() < 0.02)
            {
                // Replace with an actual unique artifact item defined elsewhere
                PackItem(new MaxxiaScroll());
            }
        }

        // --- Serialization ---
        public WitchesFamiliar(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            // Save timer state if desired; reinitialize on load as needed.
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reinitialize timers on load
            m_NextChainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_LastLocation = this.Location;
        }
    }
}
