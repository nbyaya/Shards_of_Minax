using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // Needed for spell effects and helper
using Server.Network; // Needed for effects
using Server.ContextMenus; // For GetContextMenuEntries override

namespace Server.Mobiles
{
    [CorpseName("a tormented soul corpse")]
    public class WanderingSoul : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextSorrowTime;
        private DateTime m_NextFractureTime;
        private DateTime m_NextShiftTime;
        private DateTime m_NextAuraPulse; // Timer for passive aura check

        // Unique Hue - Example: 1151 is an ethereal cyan/white.
        private const int UniqueHue = 1151;

        [Constructable]
        public WanderingSoul() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.15, 0.3) // Reactive caster AI
        {
            Name = "a Wandering Soul";
            Body = 0x3CA; // Ghostly body from RestlessSoul
            Hue = UniqueHue;

            // --- Stats Focused on Magic and Evasion ---
            SetStr(150, 200);
            SetDex(200, 250);
            SetInt(400, 500);

            SetHits(800, 1100);
            SetStam(200, 250);
            SetMana(500, 700);

            SetDamage(10, 15);

            // Damage Types: Mainly Cold and Energy
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Cold, 45);
            SetDamageType(ResistanceType.Energy, 45);

            // Resistances
            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 70, 80);

            // Skills
            SetSkill(SkillName.EvalInt, 100.1, 115.0);
            SetSkill(SkillName.Magery, 100.1, 115.0);
            SetSkill(SkillName.MagicResist, 105.1, 120.0);
            SetSkill(SkillName.Meditation, 95.0, 105.0);
            SetSkill(SkillName.Tactics, 70.1, 85.0);
            SetSkill(SkillName.Wrestling, 70.1, 85.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;
            ControlSlots = 4;

            // Initialize ability cooldowns
            m_NextSorrowTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextFractureTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18));
            m_NextShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextAuraPulse = DateTime.UtcNow + TimeSpan.FromSeconds(2);
        }

        // --- Use Sounds from RestlessSoul ---
        public override int GetIdleSound() { return 0x107; }
        public override int GetAngerSound() { return 0x1BF; }
        public override int GetDeathSound() { return 0xFD; }

        // --- Standard Properties ---
        public override bool AlwaysAttackable { get { return true; } }
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 5; } }

        public override double DispelDifficulty { get { return 125.0; } }
        public override double DispelFocus { get { return 45.0; } }

        public override void DisplayPaperdollTo(Mobile to) { }
        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i] is ContextMenus.PaperdollEntry)
                    list.RemoveAt(i--);
            }
        }

        // --- Passive Ability: Soul Leech Aura ---
        // Periodically drains stamina from nearby enemies
        public void CheckSoulLeechAura()
        {
            if (Map == null || Map == Map.Internal || !Alive)
                return;

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 3); // Short range aura

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m) && m.Alive)
                {
                    if (m is Mobile targetMobile)
                    {
                        if (targetMobile.Stam > 5)
                        {
                           targets.Add(targetMobile);
                        }
                    }
                }
            }
            eable.Free();

            if (targets.Count > 0)
            {
                // Subtle visual effect: Wisps move towards the soul
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                              0x374A, 5, 10, UniqueHue, 0, 1, 0);

                foreach (Mobile target in targets)
                {
                    int stamDrained = Utility.RandomMinMax(3, 7);
                    target.Stam -= stamDrained;
                    target.SendMessage(0x35, "You feel slightly weary near the Wandering Soul.");
                }
            }
        }

        // --- Thinking Process ---
        public override void OnThink()
        {
            base.OnThink();

            if (Map == null || Map == Map.Internal || !Alive)
                return;

            // Check passive aura periodically
            if (DateTime.UtcNow >= m_NextAuraPulse)
            {
                CheckSoulLeechAura();
                m_NextAuraPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(3, 5));
            }

            if (Combatant == null || !Alive)
                return;

            // Prioritize abilities based on cooldown and situation
            if (DateTime.UtcNow >= m_NextShiftTime && Utility.RandomDouble() < 0.3)
            {
                 SpectralShift();
                 m_NextShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (DateTime.UtcNow >= m_NextSorrowTime && this.InRange(Combatant.Location, 8))
            {
                EchoesOfSorrow();
                m_NextSorrowTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 28));
            }
            else if (DateTime.UtcNow >= m_NextFractureTime && this.InRange(Combatant.Location, 10))
            {
                 FracturedSpell();
                 m_NextFractureTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            }
        }

        // --- Unique Ability: Echoes of Sorrow (AoE Debuff/DoT) ---
        public void EchoesOfSorrow()
        {
            if (Map == null) return;

            this.PlaySound(0x107); // Use idle sound for a sorrowful moan
            this.Say("*Why... why...*"); // Flavor text
            // Visual: Cold pulse outwards
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                          0x376A, 10, 40, UniqueHue, 0, 5010, 0);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6); // 6 tile radius

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
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);

                    // Apply moderate cold damage
                    int damage = Utility.RandomMinMax(25, 40);
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);

                    // Apply temporary debuff (Dex/Int reduction)
                    int reduction = Utility.RandomMinMax(10, 20);
                    TimeSpan duration = TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));

                    if (target is Mobile targetMobile)
                    {
                         StatMod dexMod = new StatMod(StatType.Dex, "SorrowDex", -reduction, duration);
                         StatMod intMod = new StatMod(StatType.Int, "SorrowInt", -reduction, duration);
                         targetMobile.AddStatMod(dexMod);
                         targetMobile.AddStatMod(intMod);

                         targetMobile.SendMessage(0x22, "Waves of sorrow wash over you, chilling your body and mind!");
                         targetMobile.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                    }
                }
            }
        }

        // --- Unique Ability: Fractured Spell (Single Target Burst/Effect) ---
        public void FracturedSpell()
        {
            if (Combatant == null || Map == null) return;

            IDamageable targetDamageable = Combatant;

            if (!(targetDamageable is Mobile targetMobile) || !CanBeHarmful(targetMobile, false) || !SpellHelper.ValidIndirectTarget(this, targetMobile))
                 return;

            this.Say("*Fragment... unleash!*");
            this.PlaySound(0x1F1); // Curse sound
            // Visual effect on self - chaotic burst
            this.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);

            int choice = Utility.Random(3); // 0, 1, or 2

            DoHarmful(targetMobile);

            switch (choice)
            {
                case 0: // High Energy Damage
                    targetMobile.SendMessage(0x22, "A shard of raw magic sears you!");
                    int damage = Utility.RandomMinMax(60, 85);
                    AOS.Damage(targetMobile, this, damage, 0, 0, 0, 0, 100); // 100% Energy
                    Effects.SendBoltEffect(targetMobile, true, UniqueHue);
                    break;

                case 1: // Temporary Paralysis
                    targetMobile.SendMessage(0x22, "Residual magic binds your limbs!");
                    targetMobile.Paralyze(TimeSpan.FromSeconds(Utility.RandomMinMax(3, 5)));
                    targetMobile.FixedParticles(0x376A, 9, 32, 5005, UniqueHue, 0, EffectLayer.Waist);
                    break;

                case 2: // Significant Mana Drain
                    targetMobile.SendMessage(0x22, "The fractured spell rips at your concentration!");
                    int manaDrained = Utility.RandomMinMax(40, 70);
                    if (targetMobile.Mana >= manaDrained)
                    {
                        targetMobile.Mana -= manaDrained;
                        targetMobile.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                        targetMobile.PlaySound(0x1F8);
                    }
                    break;
            }
        }

        // --- Helper method for melee attack ---
        protected void DoAttack(Mobile target)
        {
            if (target != null)
                Attack(target);
        }

        // --- Unique Ability: Spectral Shift (Teleport/Ambush) ---
        public void SpectralShift()
        {
            if (Map == null)
                return;

            // Initialize mobCombatant to avoid unassigned variable issues
            Mobile mobCombatant = Combatant as Mobile;
            Mobile target = null;
            List<Mobile> potentialTargets = new List<Mobile>();

            // Check primary combatant first if valid
            if (mobCombatant != null && CanBeHarmful(mobCombatant, false) && InLOS(mobCombatant) && mobCombatant.Map == this.Map && mobCombatant.Alive)
            {
                 potentialTargets.Add(mobCombatant);
            }

            // Add other aggressors
            foreach (AggressorInfo info in this.Aggressors)
            {
                if (info.Attacker != null && (mobCombatant == null || info.Attacker != mobCombatant) && info.Attacker.Map == this.Map && info.Attacker.Alive && CanBeHarmful(info.Attacker, false) && InLOS(info.Attacker))
                {
                    if (!potentialTargets.Contains(info.Attacker))
                       potentialTargets.Add(info.Attacker);
                }
            }
            foreach (AggressorInfo info in this.Aggressed)
            {
                if (info.Defender != null && (mobCombatant == null || info.Defender != mobCombatant) && info.Defender.Map == this.Map && info.Defender.Alive && CanBeHarmful(info.Defender, false) && InLOS(info.Defender))
                {
                     if (!potentialTargets.Contains(info.Defender))
                        potentialTargets.Add(info.Defender);
                }
            }

            if (potentialTargets.Count > 0)
            {
                target = potentialTargets[Utility.Random(potentialTargets.Count)];
            }

            if (target == null) return; // No valid target found

            Point3D from = Location;
            Point3D to = target.Location;

            // Find a valid location near the target (removed extra boolean parameter)
			Point3D locTry = to;
			bool found = SpellHelper.FindValidSpawnLocation(Map, ref locTry, true);

			if (!found)
			{
				locTry = to; // reset before retry
				found = SpellHelper.FindValidSpawnLocation(Map, ref locTry, true);
			}

			if (found)
			{
				Point3D validTo = locTry;

				// Visual effect: Fade out
				Effects.SendLocationParticles(EffectItem.Create(from, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5032, 0);
				Effects.PlaySound(from, Map, 0x1FE); // Teleport sound

				Location = validTo;
				ProcessDelta(); // Update visibility

				// Visual effect: Fade in
				Effects.SendLocationParticles(EffectItem.Create(validTo, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5032, 0);
				Effects.PlaySound(validTo, Map, 0x1FE); // Teleport sound

				// Become target's combatant and attempt immediate action
				Combatant = target;
				DoHarmful(target);

				if (Utility.RandomBool()) // 50% chance to attack immediately
				{
					Timer.DelayCall(TimeSpan.FromSeconds(0.1), () => { if (Alive && Combatant == target) { DoAttack(target); } });
				}
				else
				{
					Timer.DelayCall(TimeSpan.FromSeconds(0.1), () =>
					{
						if (Alive && Combatant == target && CanBeHarmful(target, false))
						{
							Spell spell = null;
							int spellChoice = Utility.Random(3);
							switch (spellChoice)
							{
								case 0: spell = new Spells.First.MagicArrowSpell(this, null); break;
								case 1: spell = new Spells.First.MagicArrowSpell(this, null); break;
								case 2: spell = new Spells.First.MagicArrowSpell(this, null); break;
							}

							if (spell != null)
								spell.Cast();
						}
					});
				}
		}}




        // --- Death Effect: Spectral Release ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*Free... at last?*");
            Effects.PlaySound(this.Location, this.Map, GetDeathSound());
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                          0x374A, 10, 60, UniqueHue, 0, 5010, 0);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 5);

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
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(40, 60);
                    AOS.Damage(target, this, damage, 0, 50, 0, 0, 50);
                    target.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                }
            }

            // Spawn a few hazardous ground tiles (VortexTile with matching hue)
            int hazardsToDrop = Utility.RandomMinMax(2, 4);
            for (int i = 0; i < hazardsToDrop; i++)
            {
                Point3D hazardLocation = Location;
                bool foundLoc = false;
                for(int j = 0; j < 10; ++j) // Try 10 times to find nearby spot
                {
                    int xOffset = Utility.RandomMinMax(-3, 3);
                    int yOffset = Utility.RandomMinMax(-3, 3);
                    Point3D checkLoc = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                    if (Map.CanFit(checkLoc.X, checkLoc.Y, checkLoc.Z, 16, false, false))
                    {
                        hazardLocation = checkLoc;
                        foundLoc = true;
                        break;
                    }
                    else // Try average Z
                    {
                         checkLoc.Z = Map.GetAverageZ(checkLoc.X, checkLoc.Y);
                         if (Map.CanFit(checkLoc.X, checkLoc.Y, checkLoc.Z, 16, false, false))
                         {
                             hazardLocation = checkLoc;
                             foundLoc = true;
                             break;
                         }
                    }
                }

                if(foundLoc)
                {
                    VortexTile vortex = new VortexTile(); // Existing damaging tile
                    vortex.Hue = UniqueHue;
                    vortex.MoveToWorld(hazardLocation, this.Map);
                    Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration),
                                                  0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        // --- Loot ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 1);
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(1,2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));

            // Common ghostly reagents
            PackItem(new Nightshade(Utility.RandomMinMax(5, 10)));
            PackItem(new BlackPearl(Utility.RandomMinMax(5, 10)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(3, 7)));

            // Chance for a unique Malidor-themed item
            if (Utility.RandomDouble() < 0.03)
            {
                PackItem(new ArcaneGem(Utility.RandomMinMax(1, 2)));
            }
            if (Utility.RandomDouble() < 0.01)
            {
                PackItem(new DaemonBone(Utility.RandomMinMax(5, 10)));
            }
        }

        // --- Serialization ---
        public WanderingSoul(Serial serial) : base(serial)
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

            // Re-initialize timers on load/restart
            m_NextSorrowTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextFractureTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18));
            m_NextShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextAuraPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(3, 5));
        }
    }
}
