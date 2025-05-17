using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;       // For possible spell effects
using Server.Network;      // For visual effects and sounds

namespace Server.Mobiles
{
    [CorpseName("an arcane fiend corpse")]
    public class MagicFiend : BaseCreature
    {
        // --- Cooldown timers for special abilities ---
        private DateTime m_NextWarpTime;
        private DateTime m_NextTorrentTime;
        private DateTime m_NextAuraPulseTime;
        private Point3D m_LastLocation;

        // --- Unique Hue for Arcane Fiend (adjust as desired) ---
        private const int UniqueHue = 1250;

        [Constructable]
        public MagicFiend() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Arcane Fiend";
            Body = 43;           // Same body as the IceFiend
            BaseSoundID = 357;   // Same sound as the IceFiend
            Hue = UniqueHue;

            // --- Boosted Stats ---
            SetStr(500, 600);
            SetDex(250, 300);
            SetInt(500, 600);

            SetHits(1600, 1800);
            SetStam(300, 350);
            SetMana(800, 900);

            SetDamage(15, 30);
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 80);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 90, 100);

            // --- Enhanced Skills ---
            SetSkill(SkillName.EvalInt, 100.1, 120.0);
            SetSkill(SkillName.Magery, 100.1, 120.0);
            SetSkill(SkillName.MagicResist, 105.2, 120.0);
            SetSkill(SkillName.Meditation, 90.0, 105.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 20000;
            Karma = -20000;
            VirtualArmor = 80;
            ControlSlots = 5;

            // --- Initialize ability cooldown timers ---
            m_NextWarpTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextTorrentTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextAuraPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_LastLocation = this.Location;

            // --- Loot: reagents and chance for a unique artifact ---
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 15)));
            PackItem(new Nightshade(Utility.RandomMinMax(10, 15)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(10, 15)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
        }

        // --- Arcane Distortion Aura ---
        // As it moves, nearby targets are disrupted: mana is drained and minor energy damage is dealt.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.Alive && this.Alive && CanBeHarmful(m, false) && this.InRange(m.Location, 2))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int manaDrained = Utility.RandomMinMax(10, 15);
                    if (target.Mana >= manaDrained)
                    {
                        target.Mana -= manaDrained;
                        target.SendMessage(0x22, "The swirling arcane energies disrupt your magic!");
                        target.FixedParticles(0x374A, 10, 15, 5032, Hue, 0, EffectLayer.Head);
                        target.PlaySound(0x5C6);
                    }
                    AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 0, 100);
                }
            }
            base.OnMovement(m, oldLocation);
        }

        // --- Main AI Think Loop ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Prioritize abilities based on cooldowns and combat range
            if (DateTime.UtcNow >= m_NextAuraPulseTime && this.InRange(Combatant.Location, 6))
            {
                ChaosPulseAttack();
                m_NextAuraPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            else if (DateTime.UtcNow >= m_NextWarpTime && this.InRange(Combatant.Location, 8))
            {
                MagicWarpSurge();
                m_NextWarpTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
            else if (DateTime.UtcNow >= m_NextTorrentTime && this.InRange(Combatant.Location, 12))
            {
                ArcaneTorrentAttack();
                m_NextTorrentTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        // --- Unique Ability: Chaos Pulse Attack ---
        // A sudden burst that deals area energy damage and drains mana from nearby foes.
        public void ChaosPulseAttack()
        {
            if (Map == null) return;

            PlaySound(0x211);
            FixedParticles(0x3709, 10, 30, 5052, Hue, 0, EffectLayer.CenterFeet);

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

            if (targets.Count > 0)
            {
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                              0x376A, 10, 60, Hue, 0, 5039, 0);

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(30, 50);
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
                    target.FixedParticles(0x3779, 10, 25, 5032, Hue, 0, EffectLayer.Head);

                    // 50% chance to drain additional mana
                    if (Utility.RandomDouble() < 0.50)
                    {
                        if (target is Mobile targetMobile)
                        {
                            int manaDrain = Utility.RandomMinMax(20, 30);
                            if (targetMobile.Mana >= manaDrain)
                            {
                                targetMobile.Mana -= manaDrain;
                                targetMobile.SendMessage(0x22, "Your magical energy is sapped by the pulse of arcane power!");
                                targetMobile.FixedParticles(0x374A, 10, 15, 5032, Hue, 0, EffectLayer.Head);
                                targetMobile.PlaySound(0x1F8);
                            }
                        }
                    }
                }
            }
        }

        // --- Unique Ability: Magic Warp Surge ---
        // The fiend rapidly shifts its position—teleporting near its combatant—while leaving behind a magical hazard.
        public void MagicWarpSurge()
        {
            if (Combatant == null || Map == null)
                return;

            // Determine target location (if Combatant is a Mobile, use its location)
            Point3D targetLocation = Combatant.Location;
            if (Combatant is Mobile)
            {
                targetLocation = ((Mobile)Combatant).Location;
            }

            // Choose a new location near the target with a random offset (ensuring it fits)
            int offsetX = Utility.RandomMinMax(-2, 2);
            int offsetY = Utility.RandomMinMax(-2, 2);
            Point3D newLocation = new Point3D(targetLocation.X + offsetX, targetLocation.Y + offsetY, targetLocation.Z);

            if (!Map.CanFit(newLocation.X, newLocation.Y, newLocation.Z, 16, false, false))
            {
                newLocation.Z = Map.GetAverageZ(newLocation.X, newLocation.Y);
            }

            // Play pre-teleport sound at the current location
            Effects.PlaySound(this.Location, this.Map, 0x1F7);

            // Create a hazardous magical tile at the current location
            Item hazardTile;
            int randomChoice = Utility.RandomMinMax(0, 2);
            switch (randomChoice)
            {
                case 0: hazardTile = new NecromanticFlamestrikeTile(); break;
                case 1: hazardTile = new LightningStormTile(); break;
                default: hazardTile = new ManaDrainTile(); break;
            }
            hazardTile.Hue = Hue;
            hazardTile.MoveToWorld(this.Location, this.Map);

            // Teleport the fiend to the new location with visual effects
            this.Location = newLocation;
            FixedParticles(0x376A, 10, 15, 5032, Hue, 0, EffectLayer.Waist);
            PlaySound(0x1FE);
        }

        // --- Unique Ability: Arcane Torrent Attack ---
        // A chain-based attack that leaps from one target to nearby foes.
        public void ArcaneTorrentAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*Arcane Torrent!*");
            PlaySound(0x20A); // Energy bolt sound

            List<Mobile> targets = new List<Mobile>();
            Mobile initialTarget = Combatant as Mobile;
            if (initialTarget == null || !CanBeHarmful(initialTarget, false) || !SpellHelper.ValidIndirectTarget(this, initialTarget))
                return;
            targets.Add(initialTarget);

            int maxTargets = 5;
            int range = 5;

            for (int i = 0; i < maxTargets; i++)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDist = -1.0;

                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, range);
                foreach (Mobile m in eable)
                {
                    if (m != this && m != lastTarget && !targets.Contains(m) && CanBeHarmful(m, false) &&
                        SpellHelper.ValidIndirectTarget(lastTarget, m) && lastTarget.InLOS(m))
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

            for (int i = 0; i < targets.Count; i++)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x3818, 7, 0, false, false, Hue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                Mobile damageTarget = target;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(damageTarget, false))
                    {
                        DoHarmful(damageTarget);
                        int damage = Utility.RandomMinMax(25, 40);
                        AOS.Damage(damageTarget, this, damage, 0, 0, 0, 0, 100);
                        damageTarget.FixedParticles(0x374A, 1, 15, 9502, Hue, 0, EffectLayer.Head);
                    }
                });
            }
        }

        // --- Death Effect: Arcane Collapse ---
        // On death, the fiend releases a massive burst of arcane energy that spawns hazardous magical tiles.
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*The arcane energies collapse!*");
            Effects.PlaySound(this.Location, this.Map, 0x211);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                          0x3709, 10, 60, Hue, 0, 5052, 0);

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

                Item hazardTile;
                int choice = Utility.RandomMinMax(0, 4);
                switch (choice)
                {
                    case 0: hazardTile = new ChaoticTeleportTile(); break;
                    case 1: hazardTile = new FlamestrikeHazardTile(); break;
                    case 2: hazardTile = new ThunderstormTile(); break;
                    case 3: hazardTile = new NecromanticFlamestrikeTile(); break;
                    default: hazardTile = new ManaDrainTile(); break;
                }
                hazardTile.Hue = Hue;
                hazardTile.MoveToWorld(hazardLocation, this.Map);
                Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration),
                                              0x376A, 10, 20, Hue, 0, 5039, 0);
            }

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 70.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 10));

            if (Utility.RandomDouble() < 0.03) // 3% chance for a unique arcane artifact
            {
                PackItem(new ArcaneCrystal()); // Replace with your actual unique item class
            }
        }

        // --- Serialization ---
        public MagicFiend(Serial serial) : base(serial)
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

            m_NextWarpTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextTorrentTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextAuraPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
        }
    }
}
