using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;          // For potential spell effects
using Server.Network;         // For visual and sound effects

namespace Server.Mobiles
{
    [CorpseName("a yamish demon corpse")]
    public class YamishDemon : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextCorruptBurstTime;
        private DateTime m_NextSoulSiphonTime;
        private DateTime m_NextShadowRiftTime;

        // Unique hue for visual flavor (choose as desired)
        private const int DemonHue = 1353;

        // Track last location (for optional movement effects)
        private Point3D m_LastLocation;

        [Constructable]
        public YamishDemon() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Yamish Demon";
            Body = 249;                 // Same as the Yamandon body.
            BaseSoundID = 1260;         // Using Yamandon sounds for attack.
            Hue = DemonHue;

            // --- Significantly Boosted Stats & Magic Focus ---
            SetStr(900, 1100);
            SetDex(400, 500);
            SetInt(700, 850);

            SetHits(2000, 2300);
            SetStam(500, 600);
            SetMana(800, 1000);

            SetDamage(25, 40);

            // Damage types: physical and heavy arcane energy
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Energy, 60);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 80);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 80, 100);

            // Skills: a focus on magic and magic resistance
            SetSkill(SkillName.EvalInt, 120.0, 135.0);
            SetSkill(SkillName.Magery, 120.0, 135.0);
            SetSkill(SkillName.MagicResist, 130.0, 145.0);
            SetSkill(SkillName.Meditation, 110.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 115.0);
            SetSkill(SkillName.Wrestling, 100.0, 115.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 100;
            ControlSlots = 5;  // Boss-level creature

            // Initialize ability cooldown timers.
            m_NextCorruptBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSoulSiphonTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextShadowRiftTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            // Standard loot items (magic reagents).
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 15)));
            PackItem(new Nightshade(Utility.RandomMinMax(10, 15)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(10, 15)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));

            m_LastLocation = this.Location;
        }

        public YamishDemon(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement { get { return true; } }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override Poison HitPoison { get { return Utility.RandomBool() ? Poison.Deadly : Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override int Hides { get { return 30; } }

        public override int GetAttackSound() { return 1260; }
        public override int GetAngerSound()  { return 1262; }
        public override int GetDeathSound()  { return 1259; }
        public override int GetHurtSound()   { return 1263; }
        public override int GetIdleSound()   { return 1261; }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10));

            // 0.5% chance for a unique drop (replace with an actual defined item)
            if (Utility.RandomDouble() < 0.005)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        // --- Reactive Demonic Counterattack ---
        // Called when the demon is damaged by a spell or melee attack.
        public override void OnDamagedBySpell(Mobile attacker)
        {
            base.OnDamagedBySpell(attacker);
            DoDemonicCounter(attacker);
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);
            DoDemonicCounter(attacker);
        }

        private void DoDemonicCounter(Mobile attacker)
        {
            if (Map == null)
                return;

            if (attacker is BaseCreature && ((BaseCreature)attacker).BardProvoked)
                return;

            if (Utility.RandomDouble() < 0.25) // 25% chance to trigger a counter
            {
                Mobile target = null;
                if (attacker is BaseCreature creature)
                {
                    Mobile master = creature.GetMaster();
                    if (master != null)
                        target = master;
                }
                if (target == null || !target.InRange(this, 18))
                    target = attacker;

                Animate(10, 4, 1, true, false, 0);

                ArrayList targets = new ArrayList();
                IPooledEnumerable eable = target.GetMobilesInRange(8);
                foreach (Mobile m in eable)
                {
                    if (m == this || !CanBeHarmful(m))
                        continue;

                    if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team))
                        targets.Add(m);
                    else if (m.Player && m.Alive)
                        targets.Add(m);
                }
                eable.Free();

                for (int i = 0; i < targets.Count; i++)
                {
                    Mobile m = (Mobile)targets[i];
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(30, 40), 0, 0, 0, 0, 100);
                    m.FixedParticles(0x36BD, 1, 10, 0x1F78, DemonHue, 0, EffectLayer.Waist);

                    // Drain mana and heal self—always check m is Mobile before accessing its mana.
                    if (m is Mobile targetMobile)
                    {
                        int manaDrained = Utility.RandomMinMax(20, 30);
                        if (targetMobile.Mana >= manaDrained)
                        {
                            targetMobile.Mana -= manaDrained;
                            targetMobile.SendMessage(0x22, "The demon's curse drains your arcane energy!");
                        }
                        int heal = manaDrained / 2;
                        Hits += heal;
                    }
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || !Alive)
                return;

            // Ability: Corrupt Burst (AoE energy burst + mana drain)
            if (DateTime.UtcNow >= m_NextCorruptBurstTime && InRange(Combatant.Location, 8))
            {
                CorruptBurst();
                m_NextCorruptBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Ability: Soul Siphon (Chain attack that drains health and mana)
            else if (DateTime.UtcNow >= m_NextSoulSiphonTime && InRange(Combatant.Location, 10))
            {
                SoulSiphonAttack();
                m_NextSoulSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            // Ability: Shadow Rift Attack (Spawns a hazardous tile)
            else if (DateTime.UtcNow >= m_NextShadowRiftTime && InRange(Combatant.Location, 12))
            {
                ShadowRiftAttack();
                m_NextShadowRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }

            // Optional reactive ability: Phasing Malediction (teleport when at low health)
            if (Hits < (HitsMax * 0.3) && Utility.RandomDouble() < 0.1)
            {
                PhasingMalediction();
            }
        }

        // --- Unique Ability: Corrupt Burst ---
        // Damages nearby foes and drains their mana.
        public void CorruptBurst()
        {
            if (Map == null)
                return;

            PlaySound(0x211); // Magic explosion sound
            FixedParticles(0x3709, 10, 30, 5052, DemonHue, 0, EffectLayer.CenterFeet);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 6);
            foreach (Mobile m in eable)
            {
                if (m == this || !CanBeHarmful(m) || !SpellHelper.ValidIndirectTarget(this, m))
                    continue;
                targets.Add(m);
            }
            eable.Free();

            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                int damage = Utility.RandomMinMax(40, 60);
                AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
                target.FixedParticles(0x3779, 10, 25, 5032, DemonHue, 0, EffectLayer.Head);

                if (target is Mobile targetMobile)
                {
                    int manaDrained = Utility.RandomMinMax(20, 40);
                    if (targetMobile.Mana >= manaDrained)
                    {
                        targetMobile.Mana -= manaDrained;
                        targetMobile.SendMessage(0x22, "Your arcane energy is sapped by a corrupt burst!");
                        targetMobile.FixedParticles(0x374A, 10, 15, 5032, DemonHue, 0, EffectLayer.Head);
                        targetMobile.PlaySound(0x1F8);
                    }
                }
            }
        }

        // --- Unique Ability: Soul Siphon Attack ---
        // A chain attack that bounces between foes, dealing damage, draining mana, and healing the demon.
        public void SoulSiphonAttack()
        {
            if (Combatant == null || Map == null)
                return;

            if (!(Combatant is Mobile initialTarget))
                return;
            if (!CanBeHarmful(initialTarget, false) || !SpellHelper.ValidIndirectTarget(this, initialTarget))
                return;

            List<Mobile> targets = new List<Mobile>();
            targets.Add(initialTarget);
            int maxBounces = 5;
            int bounceRange = 5;

            for (int i = 0; i < maxBounces; i++)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDist = double.MaxValue;
                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, bounceRange);
                foreach (Mobile m in eable)
                {
                    if (m == this || m == lastTarget || targets.Contains(m) || !CanBeHarmful(m, false) || !SpellHelper.ValidIndirectTarget(lastTarget, m) || !lastTarget.InLOS(m))
                        continue;

                    double dist = lastTarget.GetDistanceToSqrt(m);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        nextTarget = m;
                    }
                }
                eable.Free();

                if (nextTarget != null)
                    targets.Add(nextTarget);
                else
                    break;
            }

            // Launch the chain attack with visual effects and damage.
            for (int i = 0; i < targets.Count; i++)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x3818, 7, 0, false, false, DemonHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

                Mobile damageTarget = target;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(damageTarget, false))
                    {
                        DoHarmful(damageTarget);
                        int damage = Utility.RandomMinMax(30, 50);
                        AOS.Damage(damageTarget, this, damage, 0, 0, 0, 0, 100);
                        damageTarget.FixedParticles(0x374A, 1, 15, 9502, DemonHue, 0, EffectLayer.Waist);

                        if (damageTarget is Mobile dmgTarget)
                        {
                            int manaDrained = Utility.RandomMinMax(15, 30);
                            if (dmgTarget.Mana >= manaDrained)
                            {
                                dmgTarget.Mana -= manaDrained;
                                dmgTarget.SendMessage(0x22, "Your soul is siphoned by the demon!");
                            }
                            // Heal the demon for half of the damage inflicted.
                            Hits += damage / 2;
                        }
                    }
                });
            }
        }

        // --- Unique Ability: Shadow Rift Attack ---
        // Creates a hazardous tile at the target's location that deals damage over time.
        public void ShadowRiftAttack()
        {
            if (Combatant == null || Map == null)
                return;

            IDamageable targetDamageable = Combatant;
            Point3D targetLocation;
            if (targetDamageable is Mobile targetMobile && CanBeHarmful(targetMobile, false))
                targetLocation = targetMobile.Location;
            else
                targetLocation = targetDamageable.Location;

            this.Say("*The shadows tear open!*");
            PlaySound(0x22F);

            Effects.SendLocationParticles(EffectItem.Create(targetLocation, Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, DemonHue, 0, 5039, 0);

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
                // Use a hazardous tile—here we use the NecromanticFlamestrikeTile for a dark magical effect.
                NecromanticFlamestrikeTile riftTile = new NecromanticFlamestrikeTile();
                riftTile.Hue = DemonHue;
                riftTile.MoveToWorld(spawnLoc, Map);
                Effects.PlaySound(spawnLoc, Map, 0x1F6);
            });
        }

        // --- Unique Ability: Phasing Malediction ---
        // Teleports the demon a short distance and creates an explosion upon reappearance.
        public void PhasingMalediction()
        {
            if (Map == null)
                return;

            Point3D oldLoc = this.Location;
            Point3D newLoc = new Point3D(
                this.X + Utility.RandomMinMax(-4, 4),
                this.Y + Utility.RandomMinMax(-4, 4),
                this.Z);

            if (!Map.CanFit(newLoc.X, newLoc.Y, newLoc.Z, 16, false, false))
            {
                newLoc.Z = Map.GetAverageZ(newLoc.X, newLoc.Y);
                if (!Map.CanFit(newLoc.X, newLoc.Y, newLoc.Z, 16, false, false))
                    return;
            }

            this.Location = newLoc;
            PlaySound(0x1FD); // Teleport sound effect
            FixedParticles(0x376A, 10, 15, 5032, DemonHue, 0, EffectLayer.Waist);

            // Explosion effect upon arrival
            Effects.SendLocationParticles(EffectItem.Create(newLoc, Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, DemonHue, 0, 5052, 0);

            // Damage nearby targets in a 3-tile radius.
            IPooledEnumerable eable = Map.GetMobilesInRange(newLoc, 3);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 0, 0, 100);
                }
            }
            eable.Free();
        }

        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*My essence... consumed by the void...*");
                Effects.PlaySound(Location, Map, 0x211);
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, DemonHue, 0, 5052, 0);

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
                    // Here, using ThunderstormTile as a final chaotic hazard; you could also choose any other available tile.
                    ThunderstormTile hazard = new ThunderstormTile();
                    hazard.Hue = DemonHue;
                    hazard.MoveToWorld(hazardLocation, Map);
                    Effects.SendLocationParticles(EffectItem.Create(hazardLocation, Map, EffectItem.DefaultDuration),
                        0x376A, 10, 20, DemonHue, 0, 5039, 0);
                }
            }
            base.OnDeath(c);
        }

        // --- Standard Overrides ---
        public override bool BleedImmune { get { return true; } }
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 70.0; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextCorruptBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSoulSiphonTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextShadowRiftTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}
